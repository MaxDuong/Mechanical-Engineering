using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System;
using UnityEngine;

public class OpcUa_Client : MonoBehaviour
{
    private ApplicationInstance application;
    private Session session;

    async void Start()
    {
        application = new ApplicationInstance
        {
            ApplicationName = "Unity OPC UA Client",
            ApplicationType = ApplicationType.Client,
        };

        // Load configuration
        application.LoadApplicationConfiguration(false).Wait();
        application.CheckApplicationInstanceCertificate(false, 0).Wait();

        // Connect to server
        var endpointURL = "opc.tcp://127.0.0.1:4840/freeopcua/server/";
        var endpoint = CoreClientUtils.SelectEndpoint(endpointURL, useSecurity: false);
        var config = new ConfiguredEndpoint(null, endpoint, EndpointConfiguration.Create(application.ApplicationConfiguration));

        session = await Session.Create(application.ApplicationConfiguration, config, false, "UnityClient", 60000, null, null);

        Debug.Log("Connected to OPC UA server");

        // Read variable
        var nodeId = new NodeId("ns=2;i=2"); // Replace with the actual NodeId
        var value = session.ReadValue(nodeId).Value;
        Debug.Log($"Read value: {value}");
    }

    void OnDestroy()
    {
        session.Close();
        session.Dispose();
    }
}