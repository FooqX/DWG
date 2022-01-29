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
        Console.WriteLine("- webhooks.txt " + new FileInfo(webhookPath).Length + " KB ~> 0 KB");
        FileStream fs = File.OpenWrite(webhookPath);
        fs.SetLength(0);
        fs.Dispose();
        fs.Close();
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
try {
    // Opening streamwriter
    using StreamWriter sw = File.AppendText(path: webhookPath);

    // Configuring streamwriter for performance
    sw.AutoFlush = false;
    sw.NewLine = "\n";
    
    // Generating webhooks (unbuffered)
    for (uint i = 0; i < webhookCount; i++) {
        sw.WriteLine($"https://discord.com/api/webhooks/{RandomNumber()}/{RandomToken()}");
    }

    // Buffering
    Console.WriteLine("Done writing!\nBuffering...");
    sw.Flush();  // Flushing the stream so it writes changes
    Console.WriteLine("Done buffering!");

    // Closing streamwriter
    Console.WriteLine("Disposing StreamWriter...");
    sw.Dispose();  // Releasing all resources used up by streamwriter
    Console.WriteLine("Closing StreamWriter...");
    sw.Close();  // Closing streamwriter
    Console.WriteLine("Done closing & disposing StreamWriter!\n");
} catch (Exception e) {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Error. " + e.Message);
    Console.ResetColor();

    Console.Write("Press any key to continue...");
    Console.ReadKey();
    Environment.Exit(0);
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
