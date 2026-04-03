using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient;

internal class Article
{
    public int? id {  get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string? image { get; set; }
    public string author { get; set; }
    public DateTime date { get; set; }

    public override string ToString()
    {
        return $"-------------------------------------------------------\n{id} - {title} \nDescription: {description} \nImage: {image} \nAuthor: {author} \nDate: {date}\n-------------------------------------------------------\n";
    }

}
