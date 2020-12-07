# Resource - /products

## GET

### Response

200 Ok

---

{

    "Categories":[
        "Liquor",
        "Produce",
        "Dairy",
        "Meats"
    ],
    "numberOfProductsTtoal": 8765432
    "numberOnBackorder": 13

    "_links": {
        "store-products":{
            "href": "http://localhost:1337/products?category={category}",
            "templated": true
        },
        "store-search":{
            "href": "http://localhost:1337/products?search={term}",
            "templated": true
        },
        "store-details":{
            "href": "http://localhost:1337/products/{id}",
            "templated": true
        }
    }

    

}

---

## POST


# Resopurce /products/{id}

## GET

---

{
    "id": 1,
    "name": "Dragons Milk"
    "categore": "Beer",
    "price" : 14.99,
    "count" : 5
}


---

## DELETE

