In order to run these tests you will need to add a valid client id & secret to the config file.  This is because it is incredibly difficult to mock out the authentication required for the account service.
This also makes the tests quite slow to run but I haven't found a way of substituting yet.
Don't check the updated config back in to GIT.