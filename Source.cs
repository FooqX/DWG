Console.Title = "Discord Webhooks Generator";

Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("=~=~=~=~=~=~=~=~=~=~=~=~= DWG v2.0.0 =~=~=~=~=~=~=~=~=~=~=~=~=");
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

// ~~~ Webhooks.txt file path
string webhookPath = "C:\\Users\\" + Environment.UserName + "\\Downloads\\webhooks.txt";

// ~~~ Old webhooks cleaning
if (File.Exists(webhookPath)) {
    Console.Write("Clean webhooks (Y)es/(N)o >> ");

    if ("y".Equals(Console.ReadLine().ToLower())) {
        File.WriteAllText(webhookPath, "");
        Console.WriteLine("- webhooks.txt " + new FileInfo(webhookPath).Length + " KB ~> 0 KB");
    }
} else {
    File.Create(webhookPath);
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
// maybe in these two theres a proble
string RandomNumber(int n = 18) {
    char[] r = new char[n];

    while (n-- > 0) {
        r[n] = ncharsArray[rand.Next(ncharsAvailable)];
    }

    return new string(r);
}
Console.WriteLine("\nPreparing...");
Console.WriteLine("Note: Please be patient, because this can take a while depending on webhook amount");
Console.WriteLine("\nWriting unbuffered webhooks...");
using (StreamWriter sw = File.AppendText(path: webhookPath)) {
    sw.AutoFlush = false;
    sw.NewLine = "\n";
    for (uint i = 0; i <= webhookCount; i++) {
        sw.WriteLine($"https://discord.com/api/webhooks/{RandomNumber()}/{RandomToken()}");
    }
    Console.WriteLine("Done writing!\nBuffering...");
    sw.Flush();
    Console.WriteLine("Done buffering!\n");
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Successfully generated all webhooks!");
Console.ResetColor();

Console.Write("Press any key to open webhooks file and terminate the program...");
Console.ReadKey();
using (System.Diagnostics.Process cmd = new()) {
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
}
