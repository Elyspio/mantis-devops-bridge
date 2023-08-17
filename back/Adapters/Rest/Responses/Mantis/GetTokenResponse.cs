using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MantisDevopsBridge.Api.Adapters.Rest.Responses.Mantis;

public record Attachment(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("reporter", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("reporter")]
	Reporter Reporter,
	[property: JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("created_at")]
	DateTime? CreatedAt,
	[property: JsonProperty("filename", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("filename")]
	string Filename,
	[property: JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("size")]
	int? Size,
	[property: JsonProperty("content_type", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("content_type")]
	string ContentType
);

public record Category(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name
);

public record CustomField(
	[property: JsonProperty("field", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("field")]
	Field Field,
	[property: JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("value")]
	string Value
);

public record Field(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name,
	[property: JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("label")]
	string Label
);

public record File(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("filename", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("filename")]
	string Filename
);

public record FixedInVersion(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name
);

public record Handler(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name,
	[property: JsonProperty("real_name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("real_name")]
	string RealName,
	[property: JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("email")]
	string Email
);

public record History(
	[property: JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("created_at")]
	DateTime? CreatedAt,
	[property: JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("user")]
	User User,
	[property: JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("type")]
	Type Type,
	[property: JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("message")]
	string Message,
	[property: JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("note")]
	Note Note,
	[property: JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("file")]
	File File,
	[property: JsonProperty("field", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("field")]
	Field Field,
	[property: JsonProperty("old_value", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("old_value")]
	object OldValue,
	[property: JsonProperty("new_value", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("new_value")]
	object NewValue,
	[property: JsonProperty("change", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("change")]
	string Change,
	[property: JsonProperty("relationship", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("relationship")]
	Relationship Relationship,
	[property: JsonProperty("issue", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("issue")]
	Issue Issue,
	[property: JsonProperty("view_state", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("view_state")]
	ViewState ViewState
);

public record Issue(
	[property: JsonProperty("id")]
	[property: JsonPropertyName("id")]
	int Id,
	[property: JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("summary")]
	string Summary,
	[property: JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("description")]
	string Description,
	[property: JsonProperty("project", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("project")]
	Project Project,
	[property: JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("category")]
	Category Category,
	[property: JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("version")]
	Version Version,
	[property: JsonProperty("reporter", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("reporter")]
	Reporter Reporter,
	[property: JsonProperty("handler", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("handler")]
	Handler Handler,
	[property: JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("status")]
	Status Status,
	[property: JsonProperty("resolution", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("resolution")]
	Resolution Resolution,
	[property: JsonProperty("view_state", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("view_state")]
	ViewState ViewState,
	[property: JsonProperty("priority", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("priority")]
	Priority Priority,
	[property: JsonProperty("severity", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("severity")]
	Severity Severity,
	[property: JsonProperty("reproducibility", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("reproducibility")]
	Reproducibility Reproducibility,
	[property: JsonProperty("sticky", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("sticky")]
	bool? Sticky,
	[property: JsonProperty("created_at")]
	[property: JsonPropertyName("created_at")]
	DateTime CreatedAt,
	[property: JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("updated_at")]
	DateTime? UpdatedAt,
	[property: JsonProperty("notes", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("notes")]
	IReadOnlyList<Note>? Notes,
	[property: JsonProperty("relationships", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("relationships")]
	IReadOnlyList<Relationship> Relationships,
	[property: JsonProperty("custom_fields", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("custom_fields")]
	IReadOnlyList<CustomField> CustomFields,
	[property: JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("history")]
	IReadOnlyList<History> History,
	[property: JsonProperty("attachments", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("attachments")]
	IReadOnlyList<Attachment> Attachments,
	[property: JsonProperty("steps_to_reproduce", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("steps_to_reproduce")]
	string StepsToReproduce,
	[property: JsonProperty("fixed_in_version", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("fixed_in_version")]
	FixedInVersion FixedInVersion,
	[property: JsonProperty("target_version", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("target_version")]
	TargetVersion TargetVersion,
	[property: JsonProperty("additional_information", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("additional_information")]
	string AdditionalInformation,
	[property: JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("platform")]
	string Platform,
	[property: JsonProperty("os", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("os")]
	string Os,
	[property: JsonProperty("os_build", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("os_build")]
	string OsBuild,
	[property: JsonProperty("profile", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("profile")]
	Profile Profile
);

public record Issue2(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("status")]
	Status Status,
	[property: JsonProperty("resolution", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("resolution")]
	Resolution Resolution,
	[property: JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("summary")]
	string Summary,
	[property: JsonProperty("handler", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("handler")]
	Handler Handler
);

public record Note(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int Id,
	[property: JsonProperty("reporter", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("reporter")]
	Reporter Reporter,
	[property: JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("text")]
	string Text,
	[property: JsonProperty("view_state", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("view_state")]
	ViewState ViewState,
	[property: JsonProperty("attachments", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("attachments")]
	IReadOnlyList<Attachment> Attachments,
	[property: JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("type")]
	string Type,
	[property: JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("created_at")]
	DateTime CreatedAt,
	[property: JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("updated_at")]
	DateTime UpdatedAt
);

public record Note2(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id
);

public record Priority(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name,
	[property: JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("label")]
	string Label
);

public record Profile(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("user")]
	User User,
	[property: JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("platform")]
	string Platform,
	[property: JsonProperty("os", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("os")]
	string Os,
	[property: JsonProperty("os_build", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("os_build")]
	string OsBuild,
	[property: JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("description")]
	string Description
);

public record Project(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name
);

public record Relationship(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("type")]
	Type Type,
	[property: JsonProperty("issue", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("issue")]
	Issue Issue
);

public record Relationship2(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name,
	[property: JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("label")]
	string Label
);

public record Reporter(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name,
	[property: JsonProperty("real_name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("real_name")]
	string RealName,
	[property: JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("email")]
	string Email
);

public record Reproducibility(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name,
	[property: JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("label")]
	string Label
);

public record Resolution(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name,
	[property: JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("label")]
	string Label
);

public record GetTokenResponse(
	[property: JsonProperty("issues")]
	[property: JsonPropertyName("issues")]
	IReadOnlyList<Issue> Issues
);

public record Severity(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name,
	[property: JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("label")]
	string Label
);

public record Status(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name,
	[property: JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("label")]
	string Label,
	[property: JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("color")]
	string Color
);

public record TargetVersion(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name
);

public record Type(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name,
	[property: JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("label")]
	string Label
);

public record User(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name,
	[property: JsonProperty("real_name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("real_name")]
	string RealName,
	[property: JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("email")]
	string Email
);

public record Version(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name
);

public record ViewState(
	[property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("id")]
	int? Id,
	[property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("name")]
	string Name,
	[property: JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
	[property: JsonPropertyName("label")]
	string Label
);