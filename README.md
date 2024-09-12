GoonsTracker Telegram Bot
GoonsTracker is a simple Telegram bot that alerts players about the current location of the "Goons" in Escape from Tarkov. It uses the Tarkov.dev API to fetch the data and send updates to a specified Telegram group.

Features
Fetches current Goons location from the Tarkov.dev API.
Responds to the /goons command with the Goons' current location.
Sends a start-up message to the group when the bot is launched.
Prerequisites
.NET 6 SDK
A Telegram bot (You can create one with BotFather)
API access to Tarkov.dev
Setup
1. Clone the Repository
bash
Kód másolása
git clone https://github.com/yourusername/GoonsTracker-bot.git
cd GoonsTracker-bot
2. Create a .env File
Create a .env file in the project root with the following content:

makefile
Kód másolása
TELEGRAM_BOT_TOKEN=your_telegram_bot_token_here
Make sure to replace your_telegram_bot_token_here with the actual bot token you received from BotFather.

3. Add Your Group Chat ID (Optional)
If you want the bot to send a startup message when it’s launched, you need to add the ID of your group chat. Update the Main method in Program.cs:

csharp
Kód másolása
long groupChatId = YOUR_GROUP_CHAT_ID;  // Replace with your group chat ID
You can retrieve your group chat ID by inviting the bot to the group and using debugging logs to capture it.

4. Install Dependencies
Make sure to restore the required dependencies:

bash
Kód másolása
dotnet restore
5. Run the Bot
You can run the bot with the following command:

bash
Kód másolása
dotnet run
The bot should now be live in your Telegram group!

Usage
Once the bot is running, you can use the following commands in the Telegram chat:

/goons: Fetch the current location of the Goons and display it in the chat.
Environment Variables
The bot relies on environment variables to function correctly. Here's a list of required variables:

Variable	Description
TELEGRAM_BOT_TOKEN	Your bot token from the BotFather.
YOUR_GROUP_CHAT_ID	ID of the group chat to send start-up messages (optional).
Example Environment Setup
On Windows:

bash
Kód másolása
setx TELEGRAM_BOT_TOKEN "your_bot_token"
On macOS/Linux:

bash
Kód másolása
export TELEGRAM_BOT_TOKEN="your_bot_token"
Running the Bot Locally
Run the following command to start the bot:

bash
Kód másolása
dotnet run
Deploying to Production
If deploying to a cloud service, make sure to properly configure environment variables or use secrets management (like GitHub Secrets for GitHub Actions).

Contributing
Contributions are welcome! Feel free to open issues or submit pull requests for new features, improvements, or bug fixes.

License
This project is licensed under the MIT License. See the LICENSE file for more details.
