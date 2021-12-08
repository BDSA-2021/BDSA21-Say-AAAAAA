namespace SELearning.API.Models;

public static class Migrator
{
    private static readonly string[] ContentTitles = new[]
    {
        "Wild water buffalo", "Woodrat (unidentified)", "Wattled crane", "Australian sea lion", "Capuchin, weeper"
    };

    private static readonly string[] ContentVideos = new[]
    {
        "youtube.com/watch?v=iokVvwcut5o", "youtube.com/watch?v=pNdigOmKpv8", "youtube.com/watch?v=St6NEhm-xps", "youtube.com/watch?v=jImdY7O2UdY", "youtube.com/watch?v=2FVfmlnrDW4"
    };

    public static IHost Migrate(this IHost host)
    {
        Migrator.MigrateContext<WeatherContext>(host);
        Migrator.MigrateContext<SELearningContext>(host);
        Seed(host);

        return host;
    }

    private static void MigrateContext<T>(IHost host) where T : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            using var ctx = scope.ServiceProvider.GetRequiredService<T>();

            ctx.Database.Migrate();
        }
    }

    private static void Seed(IHost host) {
        using (var scope = host.Services.CreateScope())
        {
            using var ctx = scope.ServiceProvider.GetRequiredService<SELearningContext>();

            if (!ctx.Section.Any() || !ctx.Comments.Any() || !ctx.Content.Any()) {
                var sections = AddSections(ctx);
                var content = AddContent(ctx, sections);
                AddComments(ctx, content);

                ctx.SaveChanges();
            }
        }
    }

    private static IEnumerable<Section> AddSections(SELearningContext context) {
        var sections = new List<Section> {
            new Section {
                Title = "Punk in London",
                Description = "Documentary|Musical"
            },
            new Section {
                Title = "Patton Oswalt: Finest Hour",
                Description = "Comedy"
            },
            new Section {
                Title = "Day a Pig Fell Into the Well, The (Daijiga umule pajinnal)",
                Description = "Drama"
            },
            new Section {
                Title = "52 Tuesdays",
                Description = "Children|Drama"
            },
            new Section {
                Title = "Venus Beauty Institute (Vénus beauté)",
                Description = "Comedy|Drama|Romance"
            },
        };
        context.AddRange(sections);
        return sections;
    }

    private static IEnumerable<Content> AddContent(SELearningContext context, IEnumerable<Section> sections) {
        var rng = new Random();

        var content = new List<Content>();
        
        foreach (Section section in sections) {
            for (int i = 0; i < 5; i++) {
                content.Add(new Content {
                    Section = section,
                    Author = ContentTitles[rng.Next(0, 4)],
                    Title = ContentTitles[rng.Next(0, 4)],
                    Description = "Some description",
                    VideoLink = ContentVideos[rng.Next(0,4)],
                    Rating = rng.Next(0, 5000)
                });
            }
        }

        context.Content.AddRange(content);
        return content;
    }

    private static void AddComments(SELearningContext context, IEnumerable<Content> content) {
        var rng = new Random();

        var comments = new List<Comment>();
        
        foreach (Content c in content) {
            for (int i = 0; i < 5; i++) {
                comments.Add(new Comment {
                    Author = ContentTitles[rng.Next(0, 4)],
                    Text = ContentTitles[rng.Next(0, 4)],
                    Rating = rng.Next(0, 5000),
                    Content = c
                });
            }
        }

        context.Comments.AddRange(comments);
    }
}