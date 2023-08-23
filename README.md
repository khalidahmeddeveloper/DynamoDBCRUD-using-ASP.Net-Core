# DynamoDBCRUD

DynamoDB is a cloud-hosted NoSQL database provided by Amazon Web Services (AWS). DynamoDB provides reliable performance, a managed experience, and a convenient API access to interact with it.
With DynamoDB, you can create database tables that can store and retrieve any amount of data and serve any level of request traffic. DynamoDB supports on-demand backups and can also enable point-in-time recovery of the data.

It also supports automatic expiry of items to reduce storage and associated cost.
DynamoDB is a fully managed database, which means you don't need to spin up server instances, software installations, or other maintenance tasks.

In this post, let's learn more about DynamoDB and using it from a .NET application. We will learn how to create tables and do basic Create Read, Update and Delete (CRUD) operations from the .NET application using the DynamoDB SDK.

To get started, let's create an ASP NET Web API application from the default template. If you are using the dotnet CLI, you can use dotnet new webapi command to create a new Web API application.

It will create an API application with a default WeatherForecast Controller that returns some hardcoded data. It also comes with Swagger Endpoint setup.

Let's move this hardcoded data into DynamoDB and starting retrieving it from there.

Let's add an extra property, City, to the WeatherData class to identify the city of the weather data.

# Set Up DynamoDB
To create a Table to hold the Weather Data, head off to the AWS Console and navigate to DynamoDB (by searching in the top bar, Alt+S). Make sure you are in the appropriate Region where you want to create the Table.

The core components in DynamoDB are Tables, Items and Attributes.

A Table is a collection if Items and each Item is a collection of Attributes.
To create a table, use the 'Create table' button on the DynamoDB page. It prompts you to enter the Table Name and Primary key as shown below.

# DynamoDB supports two different kinds of primary keys.

# Partition Key
→ A simple primary key, with just one attribute. Not two items can have the same partition key value
# Partition Key and Sort Key
→ A composite primary key composed of two attributes. One attribute is the partition key, and the other a sort key. It's possible for two items to have the same partition key; however, they must have different sort keys. The combination of them must be unique.

To store the WeatherFOrecast let's create a new table with the same name (you can use a different name if you want). For the Primary key, since the weather data is retrieved based on the city, we will make that the Partition key. For the Sort key, we can use the Date property.

In this case, we can only have one Item for a city with a particular date time. If we decide to store only the date part, excluding the time, then only one record for a day.
![image](https://github.com/khalidahmeddeveloper/DynamoDBCRUD/assets/8210762/7bfacf26-e0a1-450d-82d8-18faedc87be7)
To add items to the Table from the portal, use the Create Item button.

Since we specified City and the Date as the composite primary key, any item added to this table must have those two properties.
![image](https://github.com/khalidahmeddeveloper/DynamoDBCRUD/assets/8210762/c8f74ead-f0de-4216-a1f5-e8fedaaff27d)
There can be any other property on the Item and necessarily need not be the same for each item.

We can add JSON data to the table as below → On top of City and Date properties, it contains Summary and Temperature in Celsius as our WeatherForecast class.

{
  "City": "Brisbane",
  "Date": "2021-04-02T02:10:40.595Z",
  "Summary": "Warm",
  "TemperatureC": 20
}

Below is a snapshot of the Table with a few items added to it.
![image](https://github.com/khalidahmeddeveloper/DynamoDBCRUD/assets/8210762/ac0591c5-73c1-4b88-84c1-fdd7f50ed758)



