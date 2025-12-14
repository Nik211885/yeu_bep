-- Set client encoding to UTF8
SET client_encoding = 'UTF8';

DO $$
DECLARE
admin_user_id TEXT;
BEGIN
SELECT "Id" INTO admin_user_id FROM "AspNetUsers" WHERE "NormalizedUserName" = 'ADMIN';
INSERT INTO "Categories" ("Id","CountRecipe", "Title", "Slug", "Description", "Avatar", "CreatedById", "CreatedDate", "ModifiedById", "ModifiedDate","Emoji", "IsActive") VALUES
                                                                                                                                                                              (gen_random_uuid(), 0, 'Món Việt Nam', 'mon-viet-nam', 'Các món ăn truyền thống Việt Nam từ Bắc vào Nam', 'https://images.unsplash.com/photo-1559314809-0d155014e29e?w=400', admin_user_id, current_timestamp, admin_user_id, current_timestamp, '🍜', true),

                                                                                                                                                                              (gen_random_uuid(), 0, 'Món Ý', 'mon-y', 'Pasta, pizza và các món ăn Italia chính thống', 'https://images.unsplash.com/photo-1579631542720-3a87824fff86?w=400', admin_user_id, current_timestamp, admin_user_id, current_timestamp, '🍝', true),

                                                                                                                                                                              (gen_random_uuid(), 0, 'Món Nhật', 'mon-nhat', 'Sushi, ramen và ẩm thực Nhật Bản tinh tế', 'https://images.unsplash.com/photo-1579584425555-c3ce17fd4351?w=400', admin_user_id, current_timestamp, admin_user_id, current_timestamp, '🍱', true),

                                                                                                                                                                              (gen_random_uuid(), 0, 'Món Healthy', 'mon-healthy', 'Các món ăn lành mạnh, giàu dinh dưỡng', 'https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=400', admin_user_id, current_timestamp, admin_user_id, current_timestamp, '🥗', true),

                                                                                                                                                                              (gen_random_uuid(), 0, 'Món Tráng Miệng', 'mon-trang-mieng', 'Bánh ngọt, kem và các món dessert hấp dẫn', 'https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=400', admin_user_id, current_timestamp, admin_user_id, current_timestamp, '🍰', true),

                                                                                                                                                                              (gen_random_uuid(), 0, 'Món Hàn Quốc', 'mon-han-quoc', 'Kimchi, BBQ và ẩm thực Hàn Quốc đậm đà', 'https://hoctienghanquoc.org/uploads/tin-tuc/2017_11/kimchi-09012016.jpg', admin_user_id, current_timestamp, admin_user_id, current_timestamp, '🥘', true),

                                                                                                                                                                              (gen_random_uuid(), 0, 'Món Mexico', 'mon-mexico', 'Tacos, burrito và hương vị Mexico sôi động', 'https://images.unsplash.com/photo-1565299585323-38d6b0865b47?w=400', admin_user_id, current_timestamp, admin_user_id, current_timestamp, '🌮', true),

                                                                                                                                                                              (gen_random_uuid(), 0, 'Món Thái', 'mon-thai', 'Cà ri, pad thai và ẩm thực Thái Lan cay nồng', 'https://images.unsplash.com/photo-1559314809-0d155014e29e?w=400', admin_user_id, current_timestamp, admin_user_id, current_timestamp, '🍛', true),

                                                                                                                                                                              (gen_random_uuid(), 0, 'Fast Food', 'fast-food', 'Burger, sandwich và các món ăn nhanh', 'https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=400', admin_user_id, current_timestamp, admin_user_id, current_timestamp, '🍔', true),

                                                                                                                                                                              (gen_random_uuid(), 0, 'Món Soup & Lẩu', 'mon-soup-lau', 'Các loại soup, lẩu và món nước ấm áp', 'https://images.unsplash.com/photo-1547592166-23ac45744acd?w=400', admin_user_id, current_timestamp, admin_user_id, current_timestamp, '🥣', true),

                                                                                                                                                                              (gen_random_uuid(), 0, 'Đồ Uống', 'do-uong', 'Trà, cà phê, smoothie và các loại nước giải khát', 'https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?w=400', admin_user_id, current_timestamp, admin_user_id, current_timestamp, '☕', true),

                                                                                                                                                                              (gen_random_uuid(), 0, 'Dim Sum & Điểm Tâm', 'dim-sum-diem-tam', 'Há cảo, xíu mại và các món điểm tâm Trung Hoa', 'https://images.unsplash.com/photo-1563245372-f21724e3856d?w=400', admin_user_id, current_timestamp, admin_user_id, current_timestamp, '🥟', true);
END $$;