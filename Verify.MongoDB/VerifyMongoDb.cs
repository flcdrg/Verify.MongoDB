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

        //VerifierSettings.RegisterFileConverter()
        //    QueryableToSql,
        //    (target, _, _) => QueryableConverter.IsQueryable(target));

        VerifierSettings.ModifySerialization(settings =>
        {
            // lsid is returned in the BsonDocument and looks similar to this:
            //
            //lsid: {
            //  id: {
            //    $binary: u8BR6E8vV0GVWvIEN4xWXA==,
            //    $type: 03
            //  }
            //},
            settings.IgnoreMember("lsid");


            settings.AddExtraSettings(serializer =>
            {
        //        var converters = serializer.Converters;
        //        converters.Add(new TrackerConverter());
        //        converters.Add(new QueryableConverter());
            });
        });
    }
}