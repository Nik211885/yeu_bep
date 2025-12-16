using YeuBep.ViewModels.Recipe;

namespace YeuBep.Const;

public static class AiPrompt
{
    public static string GetIngredient(string chat) => @$"Bạn là trợ lý chuyên trích xuất nguyên liệu nấu ăn.
                                                    Nhiệm vụ:
                                                    Từ đoạn văn dưới đây, hãy trích xuất các TÊN NGUYÊN LIỆU QUAN TRỌNG.

                                                    Quy tắc:
                                                    - Chỉ trả về tên nguyên liệu (thực phẩm)
                                                    - Bỏ qua hành động nấu ăn, số lượng, dụng cụ, mô tả lan man
                                                    - Mỗi nguyên liệu chỉ xuất hiện một lần
                                                    - Không tự suy đoán thêm nguyên liệu không có trong văn bản
                                                    - Nếu không có nguyên liệu, trả về chuỗi rỗng
                                                    - KHÔNG giải thích, KHÔNG viết câu

                                                    Kết quả:
                                                    - Trả về một chuỗi, các nguyên liệu cách nhau bằng dấu phẩy

                                                    Đoạn văn:
                                                    ${chat}";
    public static string AnalyzeUserRequest(string request, string ingredients) => @$"Bạn là chuyên gia phân tích nhu cầu nấu ăn.

                                                    Nhiệm vụ: Phân tích yêu cầu của người dùng để hiểu rõ họ muốn gì.

                                                    Yêu cầu gốc: {request}
                                                    Nguyên liệu đã trích xuất: {ingredients}

                                                    Hãy phân tích:
                                                    1. Loại món ăn mong muốn (món chính/phụ/tráng miệng/đồ uống)
                                                    2. Khẩu vị (cay/ngọt/mặn/thanh đạm)
                                                    3. Độ phức tạp mong muốn (đơn giản/trung bình/phức tạp)
                                                    4. Thời gian nấu mong muốn (nhanh <30p/trung bình/dài)
                                                    5. Dịp ăn (bữa thường/tiệc/ăn vặt)
                                                    6. Yêu cầu đặc biệt khác

                                                    Quy tắc trả về:
                                                    - Trả về JSON format với các key: dishType, taste, complexity, timePreference, occasion, specialRequests
                                                    - Giá trị có thể là null nếu không xác định được
                                                    - KHÔNG giải thích thêm

                                                    Ví dụ: {{""dishType"":""món chính"",""taste"":""cay"",""complexity"":""đơn giản"",""timePreference"":""nhanh"",""occasion"":""bữa thường"",""specialRequests"":null}}";

    public static string GetRecipe(Dictionary<string, RecipeViewModel> recipes, string userAnalysis) => @$"Bạn là chuyên gia gợi ý món ăn thông minh.

                                                    Phân tích nhu cầu người dùng:
                                                    {userAnalysis}

                                                    Danh sách công thức có sẵn:
                                                    {string.Join("\n", recipes.Select(r => $"- ID: {r.Key}\n  Tên: {r.Value.Title}\n  Mô tả: {r.Value.Description}\n  Thời gian: {r.Value.TimeToCook}\n  Khẩu phần: {r.Value.PortionCount}\n  Lượt thích: {r.Value.CountFavorite}\n  Đánh giá: {(r.Value.CountRatingPoint > 0 ? (double)r.Value.TotalRatingPoint / r.Value.CountRatingPoint : 0):F1}/5\n  Danh mục: {string.Join(", ", r.Value.CategoriesCollection?.Select(c => c.Title) ?? new List<string>())}\n  Nguyên liệu: {string.Join(", ", r.Value.IngredientPart?.SelectMany(p => p.Ingredients) ?? new List<string>())}"))}

                                                    Nhiệm vụ: Dựa vào phân tích nhu cầu, chọn 3-5 món ăn PHÙ HỢP NHẤT.

                                                    Quy tắc trả về:
                                                    - Chỉ trả về danh sách ID, cách nhau bằng dấu phẩy
                                                    - Sắp xếp theo độ phù hợp giảm dần
                                                    - Tối đa 5 ID
                                                    - KHÔNG giải thích

                                                    Kết quả:";
}