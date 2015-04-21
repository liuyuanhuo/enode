CREATE TABLE [dbo].[Part_Command] (
    [Sequence]                BIGINT IDENTITY (1, 1) NOT NULL,
    [CommandId]               NVARCHAR (36)          NOT NULL,
    [CommandTypeCode]         INT                    NOT NULL,
    [Timestamp]               DATETIME               NOT NULL,
    [Payload]                 NVARCHAR (MAX)         NOT NULL,
    [AggregateRootTypeCode]   INT                    NOT NULL,
    [AggregateRootId]         NVARCHAR (128)          NULL,
    [Message]                 NVARCHAR (MAX)         NULL,
    [MessageTypeCode]         INT                    NOT NULL,
    CONSTRAINT [PK_Command] PRIMARY KEY CLUSTERED ([CommandId] ASC)
)
GO
CREATE TABLE [dbo].[Part_EventStream] (
    [Sequence]                BIGINT IDENTITY (1, 1) NOT NULL,
    [AggregateRootTypeCode]   INT                    NOT NULL,
    [AggregateRootId]         NVARCHAR (128)          NOT NULL,
    [Version]                 INT                    NOT NULL,
    [CommandId]               NVARCHAR (36)          NOT NULL,
    [Timestamp]               DATETIME               NOT NULL,
    [Events]                  NVARCHAR (MAX)         NOT NULL,
    CONSTRAINT [PK_EventStream] PRIMARY KEY CLUSTERED ([AggregateRootId] ASC, [Version] ASC)
)
GO
CREATE TABLE [dbo].[Part_SequenceMessagePublishedVersion] (
    [Sequence]                BIGINT IDENTITY (1, 1) NOT NULL,
    [ProcessorName]           NVARCHAR (128)         NOT NULL,
    [AggregateRootTypeCode]   INT                    NOT NULL,
    [AggregateRootId]         NVARCHAR (128)          NOT NULL,
    [PublishedVersion]        INT                    NOT NULL,
    [FinishedTime]               DATETIME               NOT NULL,
    CONSTRAINT [PK_SequenceMessagePublishedVersion] PRIMARY KEY CLUSTERED ([ProcessorName] ASC, [AggregateRootId] ASC, [PublishedVersion] ASC)
)
GO
CREATE TABLE [dbo].[Part_MessageHandleRecord] (
    [Sequence]                  BIGINT IDENTITY (1, 1) NOT NULL,
    [MessageId]                 NVARCHAR (128)          NOT NULL,
    [HandlerTypeCode]           INT                    NOT NULL,
    [MessageTypeCode]           INT                    NOT NULL,
    [AggregateRootTypeCode]     INT                    NOT NULL,
    [AggregateRootId]           NVARCHAR (128)          NULL,
    [Version]                   INT                    NULL,
    [FinishedTime]               DATETIME               NOT NULL,
    CONSTRAINT [PK_MessageHandleRecord] PRIMARY KEY CLUSTERED ([MessageId] ASC, [HandlerTypeCode] ASC)
)
GO
CREATE TABLE [dbo].[Part_Snapshot] (
    [Sequence]               BIGINT IDENTITY (1, 1)  NOT NULL,
    [AggregateRootTypeCode]  INT                     NOT NULL,
    [AggregateRootId]        NVARCHAR (36)           NOT NULL,
    [Version]                INT                     NOT NULL,
    [Payload]                VARBINARY (MAX)         NOT NULL,
    [Timestamp]              DATETIME                NOT NULL,
    CONSTRAINT [PK_Snapshot] PRIMARY KEY CLUSTERED ([AggregateRootId] ASC, [Version] ASC)
)
GO
CREATE TABLE [dbo].[Part_Lock] (
    [LockKey]                NVARCHAR (128)          NOT NULL,
    CONSTRAINT [PK_Lock] PRIMARY KEY CLUSTERED ([LockKey] ASC)
)
GO