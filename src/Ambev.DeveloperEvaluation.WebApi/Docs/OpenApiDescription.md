## Introduction
 To make use of all features here presented, you must setup some initial data to enable you to test the system from all three user perspectives:
- Customer
- Admin
- Manager

Note: Since the application requires a starting admin user to enable the client (in such case you, dear reader) start interacting with many endpoints, I've bootstrapped a starting *Admin User*, so we can use it to create other ***users***, ***products*** or any necessary data before interacting with other endpoints.

## First Steps
To setup the initial data, the first thing you should start doing is to get a fresh token from ***Auth Endpoint***.

Assuming this is a fresh installation, get the first ***token*** by using the following credentials:

```json
{
  "email": "Admin@stubmail.com",
  "password": "AdminPassword@5000"
}
```

## How to start?
Currently, the sale is one of the last processes that this system can achieve, which means that we need to build some data before achieving it.
Creating a sale is a nice start, since we can go through some user Personas and application features.

1. Get the **token** by using the provided Admin Credential above.
2. ***POST api/auth/login*** - Use the ***token*** to set the ***Authorization header***. (Right-upper if in Swagger) 
3. ***{Acting as Admin} POST api/users*** - Create an Active Customer User. (This will fire a UserRegisteredEvent) *Note:* Remember the used credentials.
4. ***{Acting as Admin} POST api/products*** - Create one or more Products. They will be used to populate the Customer cart later.
5. ***POST api/auth/login*** - Get a new token by using the previously created Active Customer credentials ***username*** and ***password*** used in step 3.
6. Use the new ***token*** received from Customer Credentials to set the ***Authorization header***.
7. ***{Acting as Customer} GET api/products*** - List all products based on your defined criteria. 
8. ***{Acting as Customer} POST api/users/me/cart*** - Get the products you want to add in the cart and create the cart with them. (This is optional, you can also create an empty cart and add the products later via Update)
9. ***{Acting as Customer} POST api/users/me/cart/checkout*** - Once you have the products you want in your cart, you can purchase all cart items. This action will create a Sale. (This will also fire a SaleCreatedEvent)
10. ***{Acting as Customer} GET api/sales/{saleId}*** - When you have created a Sale, you can interact with by using ***Sales Endpoints***.

Feel free to mess around, many other processes might be intended as a deviation from this one.

#### Notes:
- There's a possibility to create and interact with carts without the need to authenticate, this happens in the direction that in a sales system, you don't want to add friction over a user to populate its data and start buying. This can be added later, so the overall customer experience will be smooth.
- Only carts from users are allowed to proceed into checkout. This just happened due fact we will need a lot of information regarding the 'buyer' to fulfill the Sale entity. This would be fixed by requiring unauthenticated users to fulfill such things in an 'invisible checkout' screen.  