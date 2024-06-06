USER_PATH=$(shell powershell -Command "[Environment]::GetFolderPath('UserProfile')")

install:
	dotnet tool update --global dotnet-ef
	choco install mkcert
	mkcert -install
	mkcert -cert-file $(USER_PATH)/.ssl/local-cert.pem -key-file $(USER_PATH)/.ssl/local-key.pem "docker.localhost" "*.docker.localhost"

infra:
	docker compose --profile infra up -d --build

migrate:
	dotnet ef migrations bundle -o efbundle-forecast.exe --force --self-contained -p .\src\Weather.Forecast -s .\src\Weather.Api
	efbundle-forecast.exe --connection 'Host=localhost;Database=Weather.Api;Username=postgres;Password=P@ssw0rd'

up:
	docker compose --profile infra --profile backend up -d --build

prune:
	docker compose --profile infra --profile backend down -v --rmi local
	
export-realm:
	docker compose exec -u root keycloak sh /opt/keycloak/bin/kc.sh export --dir /opt/keycloak/export --users realm_file
	docker compose cp keycloak:opt/keycloak/export/. ./.keycloak/realm-config