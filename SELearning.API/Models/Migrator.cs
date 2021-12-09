namespace SELearning.API.Models;

public static class Migrator
{
    private static readonly string[] ContentTitles = new[]
    {
        "Wild water buffalo", "Woodrat (unidentified)", "Wattled crane", "Australian sea lion", "Capuchin, weeper"
    };

    private static readonly string[] ContentVideos = new[]
    {
        "https://youtube.com/embed/iokVvwcut5o", "https://youtube.com/embed/pNdigOmKpv8", "https://youtube.com/embed/St6NEhm-xps", "https://youtube.com/embed/jImdY7O2UdY", "https://youtube.com/embed/2FVfmlnrDW4"
    };

    public static IHost Migrate(this IHost host)
    {
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

    private static void Seed(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            using var ctx = scope.ServiceProvider.GetRequiredService<SELearningContext>();

            if (!ctx.Section.Any() || !ctx.Comments.Any() || !ctx.Content.Any())
            {
                var sections = AddSections(ctx);
                var content = AddContent(ctx, sections);
                AddComments(ctx, content);

                ctx.SaveChanges();
            }
        }
    }

    private static IEnumerable<Section> AddSections(SELearningContext context)
    {
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

    private static IEnumerable<Content> AddContent(SELearningContext context, IEnumerable<Section> sections)
    {
        var rng = new Random();

        var content = new List<Content>();

        foreach (Section section in sections)
        {
            for (int i = 0; i < 5; i++)
            {
                content.Add(new Content(
                    ContentTitles[rng.Next(0, 4)],
                    "Some description",
                    ContentVideos[rng.Next(0, 4)],
                    rng.Next(0, 5000),
                    new Core.User.User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = ContentTitles[rng.Next(0, 4)]
                    },
                    section
                ));
            }
        }

        context.Content.AddRange(content);
        return content;
    }

    private static void AddComments(SELearningContext context, IEnumerable<Content> content)
    {
        var rng = new Random();

        var comments = new List<Comment>();

        foreach (Content c in content)
        {
            for (int i = 0; i < 5; i++)
            {
                comments.Add(new Comment(
                    ContentTitles[rng.Next(0, 4)],
                    null,
                    rng.Next(0, 5000),
                    c,
                    new Core.User.User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = ContentTitles[rng.Next(0, 4)]
                    }
                ));
            }
        }

        context.Comments.AddRange(comments);
    }
}