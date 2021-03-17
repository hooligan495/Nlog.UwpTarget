# About

If you are using NLog in a UWP client, you can only write logs to the sandbox your application runs in. In order to support things like the broadFileSystemAccess capability, file writing has to be done with the Windows STorage APIs. This target is a subset of the FileTarget features, with the hope of expanding it to support all the features.


# TODO
+ Complete unimplemented methods
+ Documentation
+ Get naming consistent
+ Create a nuget package
+ More unit tests
+ Remove or use FileStreamCache concept it was from an earlier discussion and if we need to support metadata with a stream it could be useful