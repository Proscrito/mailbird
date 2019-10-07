## Comments

- I kept only one single project and didn't make several layers because of small code amount.
- Dependency injection seem to be overengineering here, however it's hard to imagine up-to-date c# solution without DI, so I added Autofac usage to inject the single service I have :)
- I don't download mail headres separately from bodies. It requires more architectural efforts then it is reasonable. Using this approach it is nice to have the local email storage, to save already downloaded emails, then we can download only headers (perhaps, only the new headers, using the search criteria by date), and show message content by clicking the header. If the body wasn't downloaded yet, we can download it and save locally. So, I am using the simpliest way: getting all messages using appropriate methods from Mail.dll. It is still happen in the background thread and doesn't block UI, I believe it is enough for the demonstration.
- I didn't spend much time for the error handling. There are no any strategy, fundamental logging approach or whatever. However, exceptions from the service layer shouldn't crash the application and there is a place for the basic handling. It's not enough for the real commercial project, but should be fine for small example.
- Encryption is also not used. The field exists but not connected to any logic. Implementation does not raise the complexity, just the amount of the code. Adding some additional code flows will make the project just a bit more bulky.

## The Objective

The objective of this test is to create the back-end code for a small email application, like Mailbird.

Note that the hiring decision will primarily be based on your performance on this test, and if you perform well on it there's a good chance we will offer you the position, as we only send it to candidates we find especially interesting. The final result should show:

- Your ability to learn and work with the email component we use, Mail.dll.
- Your attention to detail when reading instructions and your ability to work alone.
- Your ability to understand the principles of MVVM and use WPF data binding.
- Your ability to create and work with a multi threaded application.
- Your ability to structure your code and avoid code duplication and other code smells.

## Application Requirements

- Connect, using Mail.dll, to a mail server of the type specified in the 'Server type' combobox. The options should be 'IMAP' and 'POP3'. The encryption options should be 'Unencrypted', 'SSL/TLS' and 'STARTTLS'.
- Once connected, the app should begin downloading just the message envelopes/headers for all mail in the inbox, and display the messages in the data grid on the left as they download. The columns should at least include 'From', 'Subject' and 'Date'.
- While downloading the envelopes, the app should also be downloading the message bodies (the actual HTML/Text) of the downloaded envelopes in a separate thread(s), so they're ready to be viewed when a message is clicked.
- Clicking on a message in the data grid should select it and show the message body HTML/Text in the text box on the right side. The body should be downloaded from the server on demand if not already downloaded, or if already downloaded just shown immediately.
- The application should be thread safe and no header or message body should ever download more than once. To accomplish this some locking will likely be required.
- OPTIONAL: In addition, the UI of the app has been purposely built fast, and you are encouraged to improve upon it any way you see fit, using proper containers etc. This is a chance to show off your design and structural skills which we also consider important - but the main focus should be on the mandatory requirements.

## Mail.dll Reference

The following are direct links to the appropriate samples from the Mail.dll samples page, to help you with the implementation.

### IMAP

- [Connect to server](http://www.limilabs.com/blog/use-ssl-with-imap)
- [Download headers](http://www.limilabs.com/blog/get-email-information-from-imap-fast)
- [Download bodies](http://www.limilabs.com/blog/download-parts-of-email-message)

### POP3

- [Connect to server](http://www.limilabs.com/blog/use-ssl-with-pop3)
- [Download headers](http://www.limilabs.com/blog/get-email-headers-using-pop3-top-command) (you can assume that the TOP command is supported) 
- [Download bodies](http://www.limilabs.com/blog/get-common-email-fields-subject-text-with-pop3)

Note that the NuGet included evaluation version of Mail.dll changes the subject of some emails to "Please purchase a license" message and shows "Please purchase a license" dialogs.

## What we look for in particular

- Embrace the KISS design principle. Simple is better than complex. Not a lot of code is required to accomplish this task so be careful not to overengineer it.
- If for whatever reason you're unable to complete all of the requirements, make a note of what you didn't complete, how you would have done it, and make sure that what IS completed is working perfectly and the code is nice and clean.
- Code duplication is kept to a minimum by using inheritance and otherwise unifying code.
- Magic strings and other code smells are kept to a minimum / is non existent. The new C# language features will help here.
- The view (MainWindow.xaml) uses MVVM and data binding extensively/exclusively.
- The solution directories and files are nicely structured.
- The app is built with a focus on speed, using multiple connections to download headers and bodies.
- The app is thread safe and uses proper synchronization.
- The UI is never blocking and all expensive operations are run on other threads.
- Variables have meaningful names, and naming in general, including properties and methods, is consistent with: https://msdn.microsoft.com/en-us/library/ms229002(v=vs.110).aspx
