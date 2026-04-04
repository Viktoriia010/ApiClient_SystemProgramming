using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiClient;

internal class WorkWithApi
{
    private static readonly string URL = "http://localhost:3000/articles";
    static HttpClient _httpClient = new HttpClient();

    public async Task Run()
    {
        while (true)
        {
            Console.WriteLine("0 - Вихід ");
            Console.WriteLine("1 - Отримання всіх записів ");
            Console.WriteLine("2 - Отримання даних за ID ");
            Console.WriteLine("3 - Пошук даних за тайтлом");
            Console.WriteLine("4 - Пошук даних за автором");
            Console.WriteLine("5 - Додати article");
            Console.WriteLine("6 - Видалити article за ID");
            Console.WriteLine("7 - Оновити тайтл за ID");
            Console.Write("Введіть число: ");
            int choise = int.Parse(Console.ReadLine());
            switch (choise)
            {
                case 0:
                    return;
                case 1:
                    {
                        var data = await GetArticles(URL);
                        if (data != null)
                        {
                            foreach (var item in data)
                            {
                                Console.WriteLine(item);
                            }

                        }
                        else
                        {
                            Console.WriteLine("Запису нема");
                        }
                    }
                    break;
                case 2:
                    {
                        Console.Write("Введіть id: ");
                        int id = int.Parse(Console.ReadLine());
                        var article = GetArticleById(id);
                        Console.WriteLine(article.Result);
                    }
                    break;
                case 3:
                    {
                        Console.Write("Введіть тайтл: ");
                        string title = Console.ReadLine();
                        var article = GetArticleByTitle(title, URL);
                        Console.WriteLine(article.Result);
                    }
                    break;
                case 4:
                    {
                        Console.Write("Введіть автора: ");
                        string author = Console.ReadLine();
                        var article = GetArticleByAuthor(author, URL);
                        foreach (var item in article.Result)
                        {
                            Console.WriteLine(item);
                        }
                    }
                    break;
                case 5:
                    {
                        Console.Write("Введіть тайтл: ");
                        string t = Console.ReadLine();
                        Console.Write("Введіть опис: ");
                        string d = Console.ReadLine();
                        Console.Write("Введіть автора: ");
                        string a = Console.ReadLine();
                        Article article = new Article()
                        {
                            title = t,
                            description = d,
                            author = a,
                            date = DateTime.Now
                        }
                        ;
                        await AddArticle(article);

                    }
                    break;
                case 6:
                    {
                        Console.Write("Введіть id: ");
                        int id = int.Parse(Console.ReadLine());
                        bool successful = await DeleteArticleById(id);
                        if (successful)
                        {
                            Console.WriteLine("Успішно");
                        }
                        else
                        {
                            Console.WriteLine("Помилка");
                        }

                    }
                    break;
                case 7:
                    {
                        Console.Write("Введіть id: ");
                        int id = int.Parse(Console.ReadLine());
                        Article ar = await GetArticleById(id);
                        if (ar == null)
                        {
                            Console.WriteLine("Такого запису не існує");
                            break;
                        }
                        Console.Write("Введіть тайтл: ");
                        string title = Console.ReadLine();
                        bool successful = await PatchArticle(ar, title);
                        if (successful)
                        {
                            Console.WriteLine("Успішно");
                        }
                        else
                        {
                            Console.WriteLine("Помилка");
                        }

                    }
                    break;
            }
        }

    }
  
    async Task<bool> DeleteArticleById(int id)
    {
        var response = await _httpClient.DeleteAsync($"{URL}/{id}");
        return response.IsSuccessStatusCode;
    }
    async Task<Article> GetArticleById(int id)
    {
        var response = await _httpClient.GetStringAsync($"{URL}/{id}");
        var obj = JsonSerializer.Deserialize<Article>(response);
        return obj;
    }
    async Task<bool> PatchArticle(Article ar, string title)
    {
        ar.title = title;
        var json = JsonSerializer.Serialize(ar);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PatchAsync($"{URL}/{ar.id}", data);
        return response.IsSuccessStatusCode;
    }
     async Task<int?> AddArticle(Article ar)
    {
        var json = JsonSerializer.Serialize(ar);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(URL, data);
        if (response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            Article? article = JsonSerializer.Deserialize<Article>(result);
            if (article != null)
            {
                return article.id;
            }
        }
        return null;
    }
     async Task<List<Article>> GetArticles(string url)
    {

        using (HttpClient client = new HttpClient())
        {
            try
            {
                var response = await client.GetStringAsync(url);
                var obj = JsonSerializer.Deserialize<List<Article>>(response); 
                if (obj != null)
                {
                    return obj;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;

        }

    }

     async Task<Article> GetArticleByTitle(string title, string url)
    {
        var articles = await GetArticles(url);
        if (articles == null)
        {
            Console.WriteLine("Помилка завантаження");
            return null;
        }
        var article = articles.FirstOrDefault(p => p.title == title);
        if (article == null)
        {
            Console.WriteLine("Такого запису не існує");
            return null;
        }
        return article;
    }
     async Task<List<Article>> GetArticleByAuthor(string author, string url)
    {
        var articles = await GetArticles(url);
        if (articles == null)
        {
            Console.WriteLine("Помилка завантаження");
            return null;
        }
        var article = articles.Where(p => p.author == author).ToList();
        if (!article.Any())
        {
            Console.WriteLine("Такого запису не існує");
            return null;
        }

        return article;
    }


}
