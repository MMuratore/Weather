USER_PATH=$(shell powershell -Command "[Environment]::GetFolderPath('UserProfile')")

install:
	dotnet tool update --global dotnet-ef
	choco install mkcert
	mkcert -install
	@if not exist "$(USER_PATH)/.ssl" mkdir "$(USER_PATH)/.ssl"
	mkcert -cert-file $(USER_PATH)/.ssl/local-cert.pem -key-file $(USER_PATH)/.ssl/local-key.pem "docker.internal" "*.docker.internal"
	powershell.exe -ExecutionPolicy Bypass -File ./.local/.traefik/dns/add-hosts-entries.ps1

infra:
	docker compose --profile infra up -d --build

migrate:
	dotnet ef migrations bundle -o efbundle-forecast.exe --force --self-contained -p .\src\Weather.Forecast -s .\src\Weather.Api -c ForecastDbContext
	efbundle-forecast.exe --connection 'Host=localhost;Database=Weather.Api;Username=postgres;Password=P@ssw0rd'

up:
	docker compose --profile infra --profile backend up -d --build

prune:
	docker compose --profile infra --profile backend down -v --rmi local
	
export-realm:
	docker compose exec -u root keycloak sh /opt/keycloak/bin/kc.sh export --dir /opt/keycloak/export --users realm_file
	docker compose cp keycloak:opt/keycloak/export/. ./.local/.keycloak/realm-config