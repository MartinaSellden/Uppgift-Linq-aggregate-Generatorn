
/*
 * De olika inputs som vi vill skicka in i objekten (hemsidorna)
 */
//string[] techniques = { "   C#", "daTAbaser", "WebbuTVeCkling ", "clean Code   " };
string[] messagesToClass = { "Glöm inte att övning ger färdighet!", "Öppna boken på sida 257." };

string colorStyling;

string[] techniques = { "   C#", "daTAbaser", "WebbuTVeCkling ", "clean Code   " };

//string kurser = techniques.Aggregate("", (courseList, nextCourse) =>
//{
//var tmp = nextCourse.Trim();

//    return courseList + " " + tmp[0].ToString().ToUpper() + tmp.Substring(1).ToLower().ToString();
//});


//string tmp = technique.Trim();
//kurser += "<p>" + tmp[0].ToString().ToUpper() + tmp.Substring(1).ToLower() + "</p>\n";


if (File.Exists("color-styling"))
{
    colorStyling = File.ReadAllText("color-styling");
}
else
{
    File.WriteAllText("color-styling", "blue");
    colorStyling = "blue";
}


// Vi skapar ett objekt för att kunna hantera en hemsida
WebsiteGenerator website = new WebsiteGenerator("Klass A", messagesToClass, techniques);

// Vi skapar en hemsida som tillåter styling, vi skickar in en färg utöver andra delar
StyledWebsiteGenerator styledWebsite = new StyledWebsiteGenerator("Klass A", colorStyling, messagesToClass, techniques);

// Vi skriver ut våra hemsidor först vanliga och sedan stylade
website.PrintPage();
Console.WriteLine("-----------------------");
//styledWebsite.PrintPage();

//styledWebsite.PrintToFile();


/*
 * Vi skapar ett interface (ett "kontrakt") med de delar som vår klass måste innehålla
 */
interface Website
{
    void PrintPage();
    void PrintToFile();
}

/*
 * Vi skapar vår WebsiteGenerator klass, med denna kan vi skapa objekt senare
 * Klassen innehåller data och behavior 
 */
class WebsiteGenerator : Website
{

    /*
     * De olika egenskaperna (datat) i varje objekt
     */
    string[] messagesToClass, techniques;
    string className;
    string kurser = "";

    /*
     * En konstruktor som tillåter oss att lägga in egen data i objektens egenskaper
     */
    public WebsiteGenerator(string className, string[] messageToClass, string[] techniques)
    {
        this.className = className;
        this.messagesToClass = messageToClass;
        this.techniques = techniques;
    }

    /*
     * Flera olika metoder för att utföra diverse funktionalitet
     * virtual = tillåter oss att override:a (göra egen version utav) metoden i ärvda klasser
     */
    virtual protected string printStart()
    {
        return "<!DOCTYPE html>\n<html>\n<body>\n<main>\n";
    }

    string printWelcomeWithAggregate(string className, string[] message) 
    {
        string welcome = $"<h1> Välkomna {className}! </h1>";

        string welcomeMessage = message.Aggregate("", (messageList, nextMessage) => messageList + "\n<p><b> Meddelande: </b> " + nextMessage.ToString()+"</p>");

        return welcome + welcomeMessage;
    }
    string printWelcome(string className, string[] message)
    {
        string welcome = $"<h1> Välkomna {className}! </h1>";

        string welcomeMessage = "";

        foreach (string msg in message)
        {
            welcomeMessage += $"\n<p><b> Meddelande: </b> {msg} </p>";
        }

        return welcome + welcomeMessage;
    }
    string printKurser()
    {
        return courseGeneratorWithAggregate(this.techniques); //ändrat till vår nya metod
    }
    string printEnd()
    {
        return "</main>\n</body>\n</html>";
    }

    public void PrintPage()
    {
        Console.WriteLine(printStart());
        Console.WriteLine(printWelcomeWithAggregate(this.className, this.messagesToClass));
        Console.WriteLine(printKurser());
        Console.WriteLine(printEnd());
    }

    public string getFileName()
    {
        string websiteName = "index";
        Console.WriteLine("Enter filename for website: ");
        try
        {
            websiteName = Console.ReadLine();
        }
        catch (IOException ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
        catch (OutOfMemoryException ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
        finally
        {
            Console.WriteLine("Saving file as: " + websiteName + ".html");
        }

        return websiteName;
    }

    public void PrintToFile()
    {

        try
        {
            FileInfo fi = new FileInfo(getFileName() + ".html");
            FileStream fs = fi.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(printStart());
                sw.WriteLine(printWelcome(this.className, this.messagesToClass));
                sw.WriteLine(printKurser());
                sw.WriteLine(printEnd());

                sw.Close();
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

    }

    /*
     * En utility metod
     */

    string courseGeneratorWithAggregate(string[] techniques)
    {
        string kurser = techniques.Aggregate("", (courseList, nextCourse) =>
        {
            var tmp = nextCourse.Trim();

            return courseList + " " + tmp[0].ToString().ToUpper() + tmp.Substring(1).ToLower().ToString();
        });

        return "<p>" + kurser + " </p>\n";
    }
    
    string courseGenerator(string[] techniques)
    {

        foreach (string technique in techniques)
        {
            string tmp = technique.Trim();
            kurser += "<p>" + tmp[0].ToString().ToUpper() + tmp.Substring(1).ToLower() + "</p>\n";
        }

        return kurser;
    }
}

/*
 * Här ärver vi egenskaper och metoder ifrån WebsiteGenerator för att kunna återanvända delar i vår StyledWebsiteGenerator
 */
class StyledWebsiteGenerator : WebsiteGenerator
{
    // En extra egenskap
    string color;

    /*
     * En utökad konstruktor.
     * Vi vill lägga in alla del egenskaper som behövs i base-klassen vi ärvde ifrån
     * Och också lägga in en färg (data) i vår nya egenskap
     */
    public StyledWebsiteGenerator(string className, string color, string[] messageToClass, string[] techniques) : base(className, messageToClass, techniques)
    {
        this.color = color;
    }

    /*
     * Vi skapar en egen version av printStart (override:ar den) för att kunna få resultatet vi önskar
     */
    override protected string printStart()
    {
        return $"<!DOCTYPE html>\n<html>\n<head>\n<style>\np {{ color: {this.color}; }}\n" +
                          "</style>\n</head>\n<body>\n<main>\n";
    }
}
