﻿using FubuCore;
using Xunit;
using ST.Client;
using StoryTeller.Remotes;

namespace StoryTeller.Testing.Remotes
{
    
    public class determining_the_system_type
    {
        [Fact]
        public void when_there_is_only_one_type()
        {
            // GrammarSystem is the only type in the Samples project

            var path = ".".ToFullPath().ParentDirectory().ParentDirectory().ParentDirectory()
                .AppendPath("Storyteller.Samples");

            var controller = new RemoteController(path);
        }
    }
}