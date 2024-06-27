sequenceDiagram
participant Client
participant Endpoint
participant AggregateRoot
participant DomainEvent
participant EventHandler
participant IntegrationEvent
participant Outbox
participant HangfireJob
participant IntegrationHandler

    Client->>Endpoint: Send Request
    Endpoint->>AggregateRoot: Execute Business Logic
    AggregateRoot->>DomainEvent: Raise Domain Event
    DomainEvent-->>Endpoint: Return to Endpoint
    Endpoint->>MediatR: Publish Domain Event
    MediatR->>EventHandler: Handle Domain Event
    EventHandler->>Endpoint: Execute Side Effects (optional)
    EventHandler->>IntegrationEvent: Create Integration Event
    IntegrationEvent->>Outbox: Save to Outbox
    Outbox-->>EventHandler: Confirm Save
    
    Endpoint-->>Client: Send Response
    
    Note over HangfireJob, Outbox: Periodic Job Execution
    HangfireJob->>Outbox: Retrieve Integration Events
    Outbox->>HangfireJob: Return Integration Events
    HangfireJob->>MediatR: Publish Integration Event
    MediatR->>IntegrationHandler: Handle Integration Event
    IntegrationHandler->>ExternalService: Call External Service (optional)
    IntegrationHandler-->>HangfireJob: Confirm Handling
    HangfireJob->>Outbox: Update Event State
    
    Note over HangfireJob, Outbox: Manual Retry Mechanism
    Client->>Outbox: Trigger Manual Retry for Failed Event
    Outbox->>HangfireJob: Retrieve Failed Integration Event
    HangfireJob->>MediatR: Publish Integration Event
    MediatR->>IntegrationHandler: Handle Integration Event
    IntegrationHandler->>ExternalService: Call External Service (optional)
    IntegrationHandler-->>HangfireJob: Confirm Handling
    HangfireJob->>Outbox: Update Event State
