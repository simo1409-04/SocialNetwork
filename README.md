# Social Network Data Processing

An educational .NET data-processing project that demonstrates relational database modelling, data import and export, validation, duplicate detection, and querying with Entity Framework Core.

The application imports messages from XML and posts from JSON, validates the input data, stores valid records in SQL Server, and exports structured reports in XML and JSON formats.

## Technologies

- C#
- .NET 6
- Entity Framework Core 6
- SQL Server
- LINQ
- Newtonsoft.Json
- XML serialization
- Data Annotations

## Database Model

The project contains the following main entities:

- User
- Conversation
- UserConversation
- Friendship
- Message
- Post

`UserConversation` and `Friendship` use composite primary keys. The model also includes one-to-many and many-to-many relationships configured through Entity Framework Core.

## Main Features

- Import messages from XML
- Import posts from JSON
- Validate DTO objects using Data Annotations
- Validate date and enum values
- Verify referenced users and conversations
- Detect duplicate messages and posts
- Store valid records using Entity Framework Core
- Export users, friendship counts, and posts to XML
- Export conversations and messages chronologically to JSON
- Execute LINQ queries with projections, ordering, and relationships

## Data Processing Flow

```tex
XML / JSON input
        ↓
DTO deserialization
        ↓
Data validation
        ↓
Reference and duplicate checks
        ↓
Entity mapping
        ↓
Entity Framework Core
        ↓
SQL Server
        ↓
XML / JSON export
