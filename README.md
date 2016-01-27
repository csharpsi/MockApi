# Mock API
#### The backend mocking application.

## This README is out of date

This web app is designed to help front end developers build out their code using a real API backend which is completley configurable. You simply add the path to your API call (e.g. `/people/{id}`), the HTTP Method (e.g. `GET`) and the status code you want the backend to respond with (e.g. `OK` or `200`) and MockAPI will give you the response body you told it to as `application/json`.

### Set up
*Warning: This application does not compile with Mono*

You will need:
* .NET 4.6
* Visual Studio 2015 (comes with .NET 4.6)
* IIS (Only if you need to use SSL)

Clone this repository and open the solution in Visual Studio. Hit Ctrl+F5 to start IIS Express (should open a browser pointing to http://localhost:51680) and you're good to go!

### How it works

When you run the application, you will see that it takes you to a *Settings* page. This is where you add your route settings. Once you've added some routes, you can then make requests to the application. For example:

If you add a route `/users/{id}`, with a response status code of `OK (200)`, a HTTP method of `GET` and a response body that looks like this -

``` javascript
{
    "result": {
        "user": "Jack Daniel"
    }
}
```

Then make a request to `GET http://localhost:51680/users/123` and you will get the JSON payload exactly as you entered it. This works by using an variation of the [Dice Coefficient Algorithm](https://en.wikipedia.org/wiki/S%C3%B8rensen%E2%80%93Dice_coefficient) to fuzzy match routes when url parameters are used.

### Supported HTTP Methods

The following HTTP methods are supported:
* GET
* PUT
* POST
* DELETE

#### POST and PUT methods

The request body is ignored when using POST and PUT HTTP methods. In order to test the API endpoints, I recommend using the chrome app [postman](https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop?hl=en).



### Changing the expected status code

If you wanted to test your application code with responses other than `200 OK` then you can make a copy of the route setting and change the Status Code to something else, e.g. `Bad Request (400)`. To get this response, simply add the `?status=400` or `?status=BadRequest` query string parameter to your request. Using the example above, the request url would look like `/users/123?stats=400`, which would return the response body you entered with the status code of `400`.

### The database

MockAPI uses an embedded data store called [LiteDB](http://www.litedb.org/) which is never checked into the remote repository. If you need to inspect the data in your database, you can use the [shell](https://github.com/mbdavid/LiteDB/wiki/Shell) executable found in the `/bin/` directory. *Note: Don't use the exe in the root as this will not know where the LiteDB.dll file is and will fall over!*

### TODO

* Unit and Integration tests

