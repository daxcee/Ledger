# Ledger
## by Matthew D. Groves
### Proof that data always lives longer that the applications that access it.

This is just a little web-based "checkbook balancing" app that I wrote.

### History

Back around, say, 2005, I was struggling to keep track of my money. I didn't really have a grasp on what my balance(s) were, how much of that money was actually waiting around for someone at Columbia Gas to cash my checks, how much money I was spending dining out every month, etc. Another issue was that when I buy gas at the pump, it may only appears as $1 at my bank initially, but I know it would be much more when it actually cleared. So this made my bank's website not completely reliable to give me an idea of the exact amount of money I have.

So, I wrote a Microsoft Access...uh...program? database? Whatever. I would input every check I wrote, every purchase, every withdraw, every deposit, etc. Then, as those transactions actually cleared (as shown online via my banks' websites), I would mark them as "reconciled". Any discrepancies I would then address individually (e.g. I was double charged, I made a typo, I forgot something, my wife forgot to tell me something, etc).

That was in 2005. Since then, I've moved that data from Access 2000, into Access 2002, Access 2003, Access 2007, then into MySQL with a CakePHP app, and now into a SQLite file and an ASP.NET MVC app (which is the repo you're looking at now).

I'm open-sourcing this because a) why not, b) it's been incredibly valuable for me over the years, c) why not.

### Initial setup

Once you setup the web server (I only run it locally, of course, since there is no authentication involved), you'll need to create a SQLite database file, run the schema creation scripts (TODO) on it, and point the Web.config to the SQLite file.

Then, you'll need to create at least one ledger. A ledger typically corresponds to an account. For instance, I have a CapitalOne 360 Checking account, so I'd create a ledger for that. You can create ledgers for other checking accounts, savings accounts, mutual funds--anything where you want to transact with and track the balance.

Then, you'll need to create at least one account. An account represents a granular category of transaction. For instance, you may create an "Electric Bill" account, or maybe "AEP Electric Bill". As many accounts as you like. Each account has a broader "category", which can be whatever you want. I basically use this for "Income" and "Expense" because I used to have reports (TODO).

Now you're all set.

### Workflow

Day-to-day, everything revolves around transactions. There are three basic kinds of transactions:

* Transactions that you have not actually executed/paid yet (typically this is bills that are due in the future)
* Transactions that you have executed/paid (e.g. you swiped your card, you wrote a check, etc)
* Transactions that have "cleared" (e.g. they have showed up on your bank's website)

#### Bills ####

The way I pay bills is I wait until the last possible day to actually pay them, but I still want to act as if that money has already been spent. This maximizes my interest accrued and also allows me to reallocate for emergencies when necessary. So, to enter a bill, create a transaction with a description, negative amount, date the bill is due, account, and ledger. Leave "date paid" and "date reconciled" blank. Now this transaction will show up on the "Bills Due" screen.

With my bank, I can schedule bills to be paid on a certain date. When you do that, put the date in the "date paid" field. Note that the money is still in your account until that date, but your "Actual Balance" will decrease by the amount of the transaction.

#### All other transactions ####

For other transactions, don't enter a "date due", just enter a "date paid". So, when I swipe my card at the pump, I'll take that receipt home and enter "gas for truck", "-20.18", blank date due, "12/20/2013", blank reconciled, "Gas" account, "Capital One" ledger.

I check my bank's website every day to see if that transaction has cleared yet. When it does, I'll enter the day it clears into the "Reconciled Date" box, and click the "Reconcile" button in the "Date Reconciled" column. Blam, now the Current Balance will start catching up to the Actual Balance.

#### Actual Balance and Current Balance ####

Current Balance is just what my bank's website says has cleared. This is often not an accurate picture of how much money I have to spend. But it's a good check to make sure all your reconciliations have been caught up.

Actual Balance represents the money that you can actually spend.

What's the deal with the difference? There's lots of situations where my bank doesn't have a real-time accurate balance. I already mentioned the gas pump. Another example: *I* know that my student loan will automatically be paid every 25th. But my bank doesn't know that. I don't want to overspend and not have enough money on the 25th in my account. So I'm just telling Ledger that the money is already spoken for.

#### Fixing errors ####

Sometimes I'll just transpose numbers or make a typo or something. So, I added the 100 latest reconciled transactions to the "Home" page that you can view and edit and make fixes. This doesn't happen very often, but when it did, I would have to actually open up the SQLite database directly (I use [SQLite Administrator](http://sqliteadmin.orbmu2k.de/)), which is kinda a pain.

## Receipts catcher ##

I introduced another project to this repository: Ledger.public. This is meant to run as a totally optional, publicly exposed website for you to enter your receipts while you are away from the normal ledger site (which I run inside my home firewall).

This is a little wacky, so stay with me.

When you deploy Ledger.public (to a nice free Azure site, for instance), you'll need to specify a secret token in the Web.config (or Web.custom.config). This token is basically a password, so make sure it's a good one, and you're deploying to a site that has TLS (like a nice free https://*.azurewebsites.net site, for instance). You'll also need to specify this in the Ledger Web.config (or Web.custom.config). Then, bookmark a url like this:

https://yourreceiptsite.azurewebsites.net/?token=yersecrettoken

The storage for this is just plain text json files on the web server. These files are small in size, and transient, so they are meant to be small in number. So don't worry about it taking up too much disk space.

You can then go and start entering receipts as you spend money right on your phone. The information you need to enter is very minimal (I made this for my wife, and she would be annoyed if she had to take the extra time to select an account and a ledger and so on).

Later on, when you're home balancing your checkbook, pull up the "Import" screen from the "Admin" pulldown menu at the top right. This page will make a jsonp call to get all the receipts that are out there at the public site and show them to you. You can make corrections, select an account/ledger, and then click "Get" to transfer this transaction to your ledger. Or you can just click "Delete" (if its erroneous or a duplicate). Repeat until all the receipts are gone. Minimum work for everyone involved, and now the accuracy of your account is more likely to improve because we all know that those paper receipts often get lost or forgotten before they make it home.

## Future stuff ##

* I used to have some good reports in Access, but I never really got around to that in the web versions. Also, I started using Mint.com which is a great tool to help you categorize transactions and do budgeting. I want to add some reports back.

* It would be great if there was a more automatic/hands-off way to create the SQLite database. Maybe Fluent Migrator.

* Need a confirmation for deleting transactions

* There's a bit too much repetition of markup to deal with the Ajax stuff: consolidate into partial views or something.

## Credits ##

"Ledger" logo found via [creativecommons.org](http://creativecommons.org) and modified from the original: [The Star-Ledger logo](http://commons.wikimedia.org/wiki/File:The_Star-Ledger_logo.png)