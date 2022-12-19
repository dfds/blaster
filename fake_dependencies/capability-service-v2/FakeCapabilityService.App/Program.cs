using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("api/v1/capabilities", () => Results.Content(
    content: @"{
	    ""items"": [
            {
                ""id"": ""1"",
                ""name"": ""fake capability"",
                ""description"": ""this is fake"",
                ""rootId"": ""1"",
                ""members"": [],
                ""contexts"": [],
                ""topics"": [
                    {
                        ""id"": ""1"",
                        ""capabilityId"": ""1"",
                        ""kafkaClusterId"": ""1"",
                        ""name"": ""fake topic name"",
                        ""description"": ""this is a fake topic"",
                        ""partitions"": ""3"",
                        ""configurations"": {
                            ""fake"": ""fake value"",
                            ""retention.ms"": ""1""
                        }
                    }
                ]
            }
        ]
    }",
    contentType: "application/json",
    contentEncoding: Encoding.UTF8
));

app.MapGet("api/v1/capabilities/{id}", () => Results.Content(
    content: @"{
        ""id"": ""1"",
        ""name"": ""fake capability"",
        ""description"": ""this is fake"",
        ""rootId"": ""1"",
        ""members"": [],
        ""contexts"": [],
        ""topics"": [
            {
                ""id"": ""1"",
                ""capabilityId"": ""1"",
                ""kafkaClusterId"": ""1"",
                ""name"": ""fake topic name"",
                ""description"": ""this is a fake topic"",
                ""partitions"": ""3"",
                ""status"": ""in progress"",
                ""configurations"": {
                    ""fake"": ""fake value"",
                    ""retention.ms"": ""1""
                }
            }
        ]
    }",
    contentType: "application/json",
    contentEncoding: Encoding.UTF8
));

app.MapGet("api/v1/kafka/cluster", () => Results.Content(
    content: @"[
        {
            ""id"": ""1"",
            ""name"": ""fake cluster name"",
            ""description"": ""this is a fake cluster"",
            ""enabled"": true,
            ""clusterId"": ""1""
        }
    ]",
    contentType: "application/json",
    contentEncoding: Encoding.UTF8
));

app.MapGet("api/v1/capabilities/{id}/topics", () => Results.Content(
    content: @"{
        ""items"": [
            {
                ""id"": ""1"",
                ""capabilityId"": ""1"",
                ""kafkaClusterId"": ""1"",
                ""name"": ""fake topic name 1"",
                ""description"": ""this is a fake topic"",
                ""partitions"": ""3"",
                ""status"": ""in progress"",
                ""configurations"": {
                    ""fake"": ""fake value"",
                    ""retention.ms"": ""1""
                }
            },
            {
                ""id"": ""2"",
                ""capabilityId"": ""1"",
                ""kafkaClusterId"": ""1"",
                ""name"": ""pub.fake topic name 2"",
                ""description"": ""this is a fake topic"",
                ""partitions"": ""3"",
                ""status"": ""requested"",
                ""configurations"": {
                    ""fake"": ""fake value"",
                    ""retention.ms"": ""1""
                }
            }
        ]
    }",
    contentType: "application/json",
    contentEncoding: Encoding.UTF8
));

app.MapGet("api/v1/topics", () => Results.Content(
    content: @"{
        ""items"": [
            {
                ""id"": ""1"",
                ""capabilityId"": ""1"",
                ""kafkaClusterId"": ""1"",
                ""name"": ""fake topic name"",
                ""description"": ""this is a fake topic"",
                ""partitions"": ""3"",
                ""status"": ""in progress"",
                ""configurations"": {
                    ""fake"": ""fake value"",
                    ""retention.ms"": ""1""
                }
            },
            {
                ""id"": ""2"",
                ""capabilityId"": ""1"",
                ""kafkaClusterId"": ""1"",
                ""name"": ""fake topic name2 "",
                ""description"": ""this is a fake topic"",
                ""partitions"": ""3"",
                ""status"": ""requested"",
                ""configurations"": {
                    ""fake"": ""fake value"",
                    ""retention.ms"": ""1""
                }
            }
        ]
    }",
    contentType: "application/json",
    contentEncoding: Encoding.UTF8
));

// fake harald

app.MapGet("api/v1/connections", () => Results.Content(
    content: @"{
        ""items"": []
    }",
    contentType: "application/json",
    contentEncoding: Encoding.UTF8
));

app.Run();
