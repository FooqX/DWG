using System.Diagnostics;
Console.Title = "Discord Webhooks Generator";

Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("~~ Discord webhooks generator v2.1.2");
Console.ResetColor();

Console.Write("Webhook amount >> ");

uint webhookCount = 0;
try {
    webhookCount = uint.Parse(Console.ReadLine());
} catch (Exception e) {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Error. " + e.Message);
    Console.ResetColor();

    Console.Write("\nPress any key to continue...");
    Console.ReadKey();
    Environment.Exit(0);
}

if (webhookCount == 0) {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Error. Webhook count cannot be below zero!");
    Console.ResetColor();

    Console.Write("\nPress any key to continue...");
    Console.ReadKey();
    Environment.Exit(0);
}

// --- Webhooks.txt file path
string webhookPath = "C:\\Users\\" + Environment.UserName + "\\Downloads\\webhooks.txt";

// --- Old webhooks cleaning
if (File.Exists(webhookPath)) {
    Console.Write("Clean webhooks (Y)es/(N)o >> ");

    if ("y".Equals(Console.ReadLine().ToLower())) {
        Console.WriteLine("- webhooks.txt " + new FileInfo(webhookPath).Length + " KB ~> 0 KB");
        try {
            // Truncating file
            using FileStream fs = File.OpenWrite(webhookPath);
            fs.SetLength(0);
            fs.Close();
            fs.Dispose();
        } catch (Exception e) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error. " + e.Message);
            Console.ResetColor();

            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
} else {
    try {
        File.Create(webhookPath);
    } catch (Exception e) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error. " + e.Message);
        Console.ResetColor();

        Console.Write("\nPress any key to continue...");
        Console.ReadKey();
        Environment.Exit(0);
    }
}

// ~~~ Token rand string generator
const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
char[] charsArray = CHARS.ToCharArray();
int charsAvailable = CHARS.Length;

// ~~~ Random numberic string generator
const string NCHARS = "0123456789";
char[] ncharsArray = NCHARS.ToCharArray();
int ncharsAvailable = NCHARS.Length;

Random rand = new();
string RandomToken(int n = 68) {
    char[] r = new char[n];

    while (n-- > 0) {
        r[n] = charsArray[rand.Next(charsAvailable)];
    }

    return new string(r);
}

string RandomNumber(int n = 18) {
    char[] r = new char[n];

    while (n-- > 0) {
        r[n] = ncharsArray[rand.Next(ncharsAvailable)];
    }

    return new string(r);
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\n──────────── Generating webhooks ────────────");
Console.ResetColor();

Console.Write("Writing unbuffered webhooks...");

try {

    // Opening streamwriter with append mode
    using StreamWriter sw = File.AppendText(webhookPath);

    // Configuring streamwriter for performance
    sw.AutoFlush = false;
    sw.NewLine = "\n";

    // Generating webhooks (unbuffered)
    for (uint i = 0; i < webhookCount; i++) {
        sw.WriteLine($"https://discord.com/api/webhooks/{RandomNumber()}/{RandomToken()}");
    }

    Console.WriteLine(" Done!");

    // Buffering
    Console.Write("Buffering...");
    sw.Flush();
    Console.WriteLine(" Done!");

    // Closing streamwriter
    Console.Write("Closing StreamWriter...");
    sw.Close();
    Console.WriteLine(" Done!");

    // Disposing streamwriter
    Console.Write("Disposing StreamWriter...");
    sw.Dispose();
    Console.WriteLine(" Done!");
} catch (Exception e) {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Error. " + e.Message);
    Console.ResetColor();

    Console.Write("Press any key to continue...");
    Console.ReadKey();
    Environment.Exit(0);
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\n──────────── Information ────────────");
Console.ResetColor();

long fileSize = new FileInfo(webhookPath).Length;

Console.WriteLine("Status: Successfully generated " + webhookCount + " webhooks!");
Console.WriteLine($"File size: {fileSize} KB ({fileSize / 1024 / 1024} MB)");
Console.WriteLine("File location: " + webhookPath);

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("─────────────────────────────────────");
Console.ResetColor();

if (webhookCount == 1) {
    Console.Write("Press any key to copy the webhook and terminate the program...");
    Console.ReadKey();

    using StreamReader webhook = File.OpenText(webhookPath);
    string content = webhook.ReadLine();
    webhook.Close();
    webhook.Dispose();

    using Process cmd = new();
    cmd.StartInfo.FileName = "cmd.exe";
    cmd.StartInfo.RedirectStandardInput = true;
    cmd.StartInfo.RedirectStandardOutput = true;
    cmd.StartInfo.CreateNoWindow = true;
    cmd.StartInfo.UseShellExecute = false;
    cmd.Start();

    cmd.StandardInput.WriteLine("echo|set/p=" + content + "|clip");
    cmd.StandardInput.Flush();
    cmd.StandardInput.Close();
    cmd.WaitForExit();
    cmd.Dispose();
} else {
    Console.Write("Press any key to open webhooks file and terminate the program...");
    Console.ReadKey();

    using Process cmd = new();
    cmd.StartInfo.FileName = "cmd.exe";
    cmd.StartInfo.RedirectStandardInput = true;
    cmd.StartInfo.RedirectStandardOutput = true;
    cmd.StartInfo.CreateNoWindow = true;
    cmd.StartInfo.UseShellExecute = false;
    cmd.Start();

    cmd.StandardInput.WriteLine(webhookPath);
    cmd.StandardInput.Flush();
    cmd.StandardInput.Close();
    cmd.WaitForExit();
    cmd.Dispose();
}

// Exiting the application and sending status code zero.
Environment.Exit(0);
