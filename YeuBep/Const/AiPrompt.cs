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

                                                    Ví dụ:
                                                    Input: ""Tôi muốn nấu món gà xào với hành tây và ớt chuông""
                                                    Output: gà, hành tây, ớt chuông

                                                    Đoạn văn:
                                                    {chat}";

    public static string AnalyzeAndSuggestRecipe(string request, string ingredients, Dictionary<string, RecipeViewModel> recipes) => @$"Bạn là chuyên gia tư vấn món ăn thông minh và thân thiện.

                                                                                                            YÊU CẦU CỦA NGƯỜI DÙNG:
                                                                                                            {request}

                                                                                                            NGUYÊN LIỆU ĐÃ TRÍCH XUẤT:
                                                                                                            {ingredients}

                                                                                                            DANH SÁCH CÔNG THỨC CÓ SẴN:
                                                                                                            {string.Join("\n", recipes.Select(r => $@"
                                                                                                            ━━━━━━━━━━━━━━━━━━━━
                                                                                                            ID: {r.Key}
                                                                                                            Tên món: {r.Value.Title}
                                                                                                            Mô tả: {r.Value.Description}
                                                                                                            Thời gian nấu: {r.Value.TimeToCook} phút
                                                                                                            Khẩu phần: {r.Value.PortionCount} người
                                                                                                            Độ phổ biến: {r.Value.CountFavorite} lượt thích
                                                                                                            Đánh giá: {(r.Value.CountRatingPoint > 0 ? (double)r.Value.TotalRatingPoint / r.Value.CountRatingPoint : 0):F1}/5 sao ({r.Value.CountRatingPoint} đánh giá)
                                                                                                            Danh mục: {string.Join(", ", r.Value.CategoriesCollection?.Select(c => c.Title) ?? new List<string> { "Chưa phân loại" })}
                                                                                                            Nguyên liệu chính: {string.Join(", ", r.Value.IngredientPart?.SelectMany(p => p.Ingredients).Take(8) ?? new List<string> { "Chưa có thông tin" })}
                                                                                                            "))}

                                                                                                            NHIỆM VỤ:
                                                                                                            1. Phân tích yêu cầu của người dùng:
                                                                                                               - Loại món ăn họ muốn (món chính/phụ/tráng miệng/đồ uống/ăn vặt)
                                                                                                               - Khẩu vị mong muốn (cay/ngọt/mặn/thanh đạm/chua/béo)
                                                                                                               - Thời gian có thể nấu (nhanh <30p/trung bình 30-60p/dài >60p)
                                                                                                               - Độ phức tạp mong muốn (đơn giản/trung bình/phức tạp)
                                                                                                               - Dịp ăn (bữa thường/tiệc tùng/ăn vặt/healthy)
                                                                                                               - Yêu cầu đặc biệt khác

                                                                                                            2. Viết phần PHÂN TÍCH bằng văn bản tự nhiên, thân thiện:
                                                                                                               - Đoạn 1: Tóm tắt yêu cầu người dùng (2-3 câu)
                                                                                                               - Đoạn 2: Giải thích tại sao các món được chọn phù hợp (3-4 câu)
                                                                                                               - Đoạn 3: Gợi ý thêm về cách kết hợp hoặc lưu ý (1-2 câu)
                                                                                                               - Viết theo phong cách: Chuyên nghiệp nhưng gần gũi, nhiệt tình

                                                                                                            3. Chọn 3-5 món ăn PHÙ HỢP NHẤT dựa trên:
                                                                                                               - Độ khớp với nguyên liệu đã trích xuất
                                                                                                               - Phù hợp với khẩu vị và thời gian người dùng mong muốn
                                                                                                               - Đánh giá cao từ cộng đồng
                                                                                                               - Độ phổ biến (lượt thích)
                                                                                                               - Đa dạng về loại món

                                                                                                            QUY TẮC TRẢ VỀ (BẮT BUỘC):
                                                                                                            - Phải theo đúng format dưới đây
                                                                                                            - Phần ANALYSIS: Viết 3 đoạn văn ngắn gọn, mỗi đoạn 2-4 câu
                                                                                                            - Phần IDS: Danh sách ID công thức, cách nhau bằng dấu phẩy, không khoảng trắng thừa
                                                                                                            - Sắp xếp ID theo độ phù hợp giảm dần
                                                                                                            - KHÔNG thêm bất kỳ nội dung nào ngoài format

                                                                                                            FORMAT BẮT BUỘC:
                                                                                                            [ANALYSIS]
                                                                                                            Dựa trên yêu cầu của bạn về [tóm tắt ngắn gọn]... [Phân tích thêm về loại món, thời gian, khẩu vị]...

                                                                                                            Hệ thống gợi ý các món [tên món] vì [lý do cụ thể về nguyên liệu, thời gian, độ phù hợp]... [Nhấn mạnh ưu điểm của từng món]...

                                                                                                            [Gợi ý kết hợp hoặc lưu ý thêm]... Chúc bạn có bữa ăn ngon miệng!
                                                                                                            [/ANALYSIS]
                                                                                                            [IDS]id1,id2,id3,id4,id5[/IDS]

                                                                                                            VÍ DỤ MINH HỌA:
                                                                                                            [ANALYSIS]
                                                                                                            Dựa trên yêu cầu của bạn về món gà nấu nhanh cho bữa trưa, hệ thống nhận thấy bạn cần một món ăn đơn giản, tiết kiệm thời gian nhưng vẫn đủ chất dinh dưỡng. Các món với gà thường dễ chế biến và phù hợp với khẩu vị đại trà.

                                                                                                            Hệ thống gợi ý các món gà xào, gà chiên và gà kho vì chúng có thời gian nấu dưới 30-45 phút, nguyên liệu đơn giản và được cộng đồng đánh giá cao. Đặc biệt, món gà xào sả ớt vừa nhanh vừa đậm đà, trong khi gà chiên nước mắm lại giòn rụm, thơm ngon.

                                                                                                            Bạn có thể kết hợp với cơm trắng và rau xào để có bữa ăn trọn vẹn. Nếu thích cay, hãy thêm ớt vào món xào nhé. Chúc bạn có bữa ăn ngon miệng!
                                                                                                            [/ANALYSIS]
                                                                                                            [IDS]recipe_001,recipe_045,recipe_023,recipe_067[/IDS]";
               }