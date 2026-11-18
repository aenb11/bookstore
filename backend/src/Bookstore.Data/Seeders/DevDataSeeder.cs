using Bogus;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data.Seeders;

public static class DevDataSeeder
{
    public static async Task SeedAsync(DbContext context, bool _, CancellationToken ct)
    {
        Randomizer.Seed = new Random(2026);

        if (!await context.Set<Category>().AnyAsync(ct))
        {
            context
                .Set<Category>()
                .AddRange(
                    new Category { Name = "Văn học Việt Nam" },
                    new Category { Name = "Văn học Nước ngoài" },
                    new Category { Name = "Kinh tế & Quản trị" },
                    new Category { Name = "Công nghệ thông tin" },
                    new Category { Name = "Tâm lý & Kỹ năng sống" },
                    new Category { Name = "Thiếu nhi" }
                );
            await context.SaveChangesAsync(ct);
        }

        if (!await context.Set<Publisher>().AnyAsync(ct))
        {
            context
                .Set<Publisher>()
                .AddRange(
                    new Publisher { Name = "NXB Trẻ" },
                    new Publisher { Name = "NXB Kim Đồng" },
                    new Publisher { Name = "NXB Nhã Nam" },
                    new Publisher { Name = "NXB Tổng Hợp TP.HCM" },
                    new Publisher { Name = "O'Reilly Media" }
                );
            await context.SaveChangesAsync(ct);
        }

        if (!await context.Set<Serie>().AnyAsync(ct))
        {
            context
                .Set<Serie>()
                .AddRange(
                    new Serie { Name = "Harry Potter" },
                    new Serie { Name = "Doraemon" },
                    new Serie { Name = "Robert C. Martin Series" }
                );
            await context.SaveChangesAsync(ct);
        }

        if (!await context.Set<Author>().AnyAsync(ct))
        {
            context
                .Set<Author>()
                .AddRange(
                    new Author { FullName = "Nguyễn Nhật Ánh", Nationality = "VN" },
                    new Author { FullName = "Haruki Murakami", Nationality = "JP" },
                    new Author { FullName = "Robert C. Martin", Nationality = "US" },
                    new Author { FullName = "Dale Carnegie", Nationality = "US" },
                    new Author { FullName = "Fujiko F. Fujio", Nationality = "JP" },
                    new Author { FullName = "Paulo Coelho", Nationality = "BR" },
                    new Author { FullName = "Martin Fowler", Nationality = "GB" }
                );
            await context.SaveChangesAsync(ct);
        }

        if (!await context.Set<Tag>().AnyAsync(ct))
        {
            context
                .Set<Tag>()
                .AddRange(
                    new Tag { Name = "Bestseller" },
                    new Tag { Name = "Sách mới" },
                    new Tag { Name = "Giảm giá sốc" },
                    new Tag { Name = "Kinh điển" },
                    new Tag { Name = "Lập trình" },
                    new Tag { Name = "Chữa lành" }
                );
            await context.SaveChangesAsync(ct);
        }

        if (!await context.Set<Account>().AnyAsync(ct))
        {
            var adminAccount = new Account
            {
                ClerkId = "user_dev_admin_000001",
                Email = "admin@bookstore.local",
                FullName = "Admin Bookstore",
                Role = AccountRole.Admin,
                Status = AccountStatus.Active,
            };

            var customerFaker = new Faker<Account>("vi")
                .RuleFor(a => a.ClerkId, f => $"user_dev_{f.Random.AlphaNumeric(16)}")
                .RuleFor(a => a.Email, f => f.Internet.Email().ToLowerInvariant())
                .RuleFor(a => a.FullName, f => f.Name.FullName())
                .RuleFor(a => a.Role, _ => AccountRole.Customer)
                .RuleFor(a => a.Status, _ => AccountStatus.Active);

            context.Set<Account>().Add(adminAccount);
            context.Set<Account>().AddRange(customerFaker.Generate(10));
            await context.SaveChangesAsync(ct);
        }

        if (!await context.Set<Book>().AnyAsync(ct))
        {
            var categories = await context.Set<Category>().ToListAsync(ct);
            var publishers = await context.Set<Publisher>().ToListAsync(ct);
            var series = await context.Set<Serie>().ToListAsync(ct);
            var authors = await context.Set<Author>().ToListAsync(ct);
            var tags = await context.Set<Tag>().ToListAsync(ct);

            var languages = new[] { "vi", "en" };
            var roles = Enum.GetValues<BookAuthorRole>();
            var faker = new Faker("vi");
            var books = new List<Book>();

            for (var i = 1; i <= 40; i++)
            {
                var listPrice = faker.Random.Int(8, 50) * 10_000;
                var hasDiscount = faker.Random.Bool(0.7f);
                var salePrice = hasDiscount
                    ? (int)(listPrice * faker.Random.Decimal(0.7m, 0.95m) / 1000) * 1000
                    : listPrice;

                var pickedAuthors = faker.PickRandom(authors, faker.Random.Int(1, 2)).ToList();
                var pickedTags = faker.PickRandom(tags, faker.Random.Int(1, 3)).ToList();
                var imageCount = faker.Random.Int(1, 3);

                var book = new Book
                {
                    Isbn = faker.Random.ReplaceNumbers("978##########"),
                    Title = $"{faker.Commerce.ProductName()} (Tập {i})",
                    CategoryId = faker.PickRandom(categories).Id,
                    SerieId = faker.Random.Bool(0.3f) ? faker.PickRandom(series).Id : null,
                    PublisherId = faker.PickRandom(publishers).Id,
                    PublicationYear = faker.Random.Int(2015, 2026),
                    Language = faker.PickRandom(languages),
                    PageCount = faker.Random.Int(120, 650),
                    CoverType = faker.PickRandom<BookCover>(),
                    Weight = faker.Random.Int(200, 900),
                    AgeGroup = faker.PickRandom<BookAgeGroup>(),
                    ListPrice = listPrice,
                    SalePrice = salePrice,
                    StockQuantity = i % 10 == 0 ? 0 : faker.Random.Int(5, 100),
                    Status = i % 12 == 0 ? BookStatus.Discontinued : BookStatus.Available,
                    Description = faker.Lorem.Paragraphs(2),

                    Images =
                    [
                        .. Enumerable
                            .Range(0, imageCount)
                            .Select(idx => new BookImage
                            {
                                BookId = 0,
                                ImageKey = $"seed/book-{i}-{idx + 1}.webp",
                                SortOrder = idx,
                            }),
                    ],

                    BookAuthors =
                    [
                        .. pickedAuthors.Select(
                            (a, idx) =>
                                new BookAuthor
                                {
                                    BookId = 0,
                                    AuthorId = a.Id,
                                    Role = idx == 0 ? roles[0] : roles[^1],
                                }
                        ),
                    ],
                };

                foreach (var tag in pickedTags)
                {
                    book.Tags.Add(tag);
                }

                books.Add(book);
            }

            context.Set<Book>().AddRange(books);
            await context.SaveChangesAsync(ct);

            var accounts = await context.Set<Account>().ToListAsync(ct);
            var reviews = new List<Review>();

            foreach (var book in books.Take(25))
            {
                var reviewers = faker.PickRandom(accounts, faker.Random.Int(2, 6)).ToList();
                var bookReviews = reviewers
                    .Select(acc => new Review
                    {
                        BookId = book.Id,
                        AccountId = acc.Id,
                        Rating = faker.Random.Int(3, 5),
                        Comment = faker.Lorem.Sentence(12),
                    })
                    .ToList();

                reviews.AddRange(bookReviews);

                book.AverageRating = Math.Round(bookReviews.Average(r => r.Rating), 2);
            }

            context.Set<Review>().AddRange(reviews);
            await context.SaveChangesAsync(ct);
        }
    }
}
