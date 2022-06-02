namespace VerifyTests;

public static class VerifyMongoDb
{
    public static void Enable()
    {
        VerifierSettings.RegisterJsonAppender(_ =>
        {
            var entries = LogCommandInterceptor.Stop();
            if (entries is null)
            {
                return null;
            }

            return new ToAppend("mongo", entries);
        });

        // lsid is returned in the BsonDocument and looks similar to this:
        //
        //lsid: {
        //  id: {
        //    $binary: u8BR6E8vV0GVWvIEN4xWXA==,
        //    $type: 03
        //  }
        //},
        // Ignore as the binary value changes each time
        VerifierSettings.IgnoreMember("lsid");
    }
}