﻿/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/
using System;
using System.Collections.Generic;

using NUnitLite;
using NUnit.Framework;

using org.puremvc.csharp.interfaces;
using org.puremvc.csharp.patterns.proxy;

namespace org.puremvc.csharp.core
{
    /**
	 * Test the PureMVC Model class.
	 */
    [TestFixture]
    public class ModelTest : TestCase
    {
         /**
  		 * Constructor.
  		 * 
  		 * @param methodName the name of the test method an instance to run
  		 */
        public ModelTest(String methodName)
            : base(methodName)
        { }

        /**
		 * Create the TestSuite.
		 */
        public static ITest Suite
        {
            get
            {
                TestSuite ts = new TestSuite(typeof(ModelTest));

                ts.AddTest(new ModelTest("testGetInstance"));
                ts.AddTest(new ModelTest("testRegisterAndRetrieveProxy"));
                ts.AddTest(new ModelTest("testRegisterAndRemoveProxy"));
				ts.AddTest(new ModelTest("testHasProxy"));
				ts.AddTest(new ModelTest("testOnRegisterAndOnRemove"));

				return ts;
            }
        }

        /**
  		 * Tests the Model Singleton Factory Method 
  		 */
  		public void testGetInstance()
        {
   			// Test Factory Method
   			IModel model = Model.getInstance();
   			
   			// test assertions
            Assert.NotNull(model, "Expecting instance not null");
            Assert.True(model is IModel, "Expecting instance implements IModel");
   		}

  		/**
  		 * Tests the proxy registration and retrieval methods.
  		 * 
  		 * <P>
  		 * Tests <code>registerProxy</code> and <code>retrieveProxy</code> in the same test.
  		 * These methods cannot currently be tested separately
  		 * in any meaningful way other than to show that the
  		 * methods do not throw exception when called. </P>
  		 */
  		public void testRegisterAndRetrieveProxy()
        {
   			// register a proxy and retrieve it.
   			IModel model = Model.getInstance();
			model.registerProxy(new Proxy("colors", new List<String>(new string[] { "red", "green", "blue" })));
			IProxy proxy = model.retrieveProxy("colors");
			List<String> data = (List<String>) proxy.getData();
			
			// test assertions
            Assert.NotNull(data, "Expecting data not null");
			Assert.True(data is List<String>, "Expecting data type is ArrayList");
   			Assert.True(data.Count == 3, "Expecting data.length == 3");
   			Assert.True(data[0].ToString() == "red", "Expecting data[0] == 'red'");
            Assert.True(data[1].ToString() == "green", "Expecting data[1] == 'green'");
            Assert.True(data[2].ToString() == "blue", "Expecting data[2] == 'blue'");
   		}
  		
  		/**
  		 * Tests the proxy removal method.
  		 */
  		public void testRegisterAndRemoveProxy()
        {
   			// register a proxy, remove it, then try to retrieve it
   			IModel model = Model.getInstance();
			model.registerProxy(new Proxy("sizes", new List<int>(new int[] { 7, 13, 21 })));
			
            IProxy removedProxy = model.removeProxy("sizes");

            Assert.True(removedProxy.getProxyName() == "sizes", "Expecting removedProxy.getProxyName() == 'sizes'");

			IProxy proxy = model.retrieveProxy("sizes");
			
			// test assertions
   			Assert.Null(proxy, "Expecting proxy is null");
   		}
  		
  		/**
  		 * Tests the hasProxy Method
  		 */
  		public void testHasProxy() {
  			
   			// register a proxy
   			IModel model = Model.getInstance();
			IProxy proxy = new Proxy("aces", new List<String>(new string[] { "clubs", "spades", "hearts", "diamonds" }));
			model.registerProxy(proxy);
			
   			// assert that the model.hasProxy method returns true
   			// for that proxy name
   			Assert.True(model.hasProxy("aces") == true, "Expecting model.hasProxy('aces') == true");
			
			// remove the proxy
			model.removeProxy("aces");
			
   			// assert that the model.hasProxy method returns false
   			// for that proxy name
   			Assert.True(model.hasProxy("aces") == false, "Expecting model.hasProxy('aces') == false");
   		}
  		
		/**
		 * Tests that the Model calls the onRegister and onRemove methods
		 */
		public void testOnRegisterAndOnRemove() {
			
  			// Get the Singleton View instance
  			IModel model = Model.getInstance();

			// Create and register the test mediator
			IProxy proxy = new ModelTestProxy();
			model.registerProxy(proxy);

			// assert that onRegsiter was called, and the proxy responded by setting its data accordingly
   			Assert.True(proxy.getData() == ModelTestProxy.ON_REGISTER_CALLED, "Expecting proxy.getData() == ModelTestProxy.ON_REGISTER_CALLED");
			
			// Remove the component
			model.removeProxy(ModelTestProxy.NAME);
			
			// assert that onRemove was called, and the proxy responded by setting its data accordingly
   			Assert.True(proxy.getData() == ModelTestProxy.ON_REMOVE_CALLED, "Expecting proxy.getData() == ModelTestProxy.ON_REMOVE_CALLED");
		}
    }
}
