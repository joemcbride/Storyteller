﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;
using ST.Client;
using Storyteller.Core.Commands;
using Storyteller.Core.Messages;
using Storyteller.Core.Remotes;
using Storyteller.Core.Remotes.Messaging;

namespace Storyteller.Core.Testing.ST
{
    [TestFixture]
    public class ClientConnectorTester
    {
        private RecordingCommand<RunSpec> theCommand;
        private IRemoteController theRemoteController;
        private ClientConnector theConnector;

        [SetUp]
        public void SetUp()
        {
            theCommand = new RecordingCommand<RunSpec>();
            theRemoteController = MockRepository.GenerateMock<IRemoteController>();

            theConnector = new ClientConnector(theRemoteController, new ICommand[] {theCommand});
        }

        [Test]
        public void calls_to_the_handler_if_one_matches_the_json()
        {
            var message = new RunSpec {id = "foo"};
            var json = JsonSerialization.ToCleanJson(message);

            theConnector.HandleJson(json);

            theCommand.Received.Single()
                .id.ShouldEqual("foo");

            theRemoteController.AssertWasNotCalled(x => x.SendJsonMessage(json));
        }

        [Test]
        public void delegates_to_the_remote_controller_if_no_matching_handler()
        {
            var json = "{foo: 1}";

            theConnector.HandleJson(json);

            theRemoteController.AssertWasCalled(x => x.SendJsonMessage(json));
        }
    }

    public class RecordingCommand<T> : Command<T> where T : ClientMessage
    {
        public readonly IList<T> Received = new List<T>();

        protected override void HandleMessage(T message)
        {
            Received.Add(message);
        }
    }
}