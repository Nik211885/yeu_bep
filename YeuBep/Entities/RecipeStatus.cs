using System.Text.Json.Serialization;
using YeuBep.Attributes.Table;

namespace YeuBep.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RecipeStatus
{
    [EnumColumnTable(LabelLevel.Warning, "Chờ phê duyệt")]
    Send,
    [EnumColumnTable(LabelLevel.Primary, "Đã phê duyệt")]
    Accept,
    [EnumColumnTable(LabelLevel.Danger, "Từ chối")]
    Reject,
    [EnumColumnTable(LabelLevel.Info, "Nháp")]
    Draft
}