create table users
(
    id           int auto_increment
        primary key,
    username     varchar(25)                           null,
    email        varchar(50)                           null,
    passwordHash varchar(255)                          null,
    role         varchar(20) default 'User'            null,
    createdAt    datetime    default CURRENT_TIMESTAMP null,
    lastLogin    datetime                              null,
    isEnabled    tinyint(1)  default 1                 null,
    constraint users_pk_2
        unique (id)
);

create table products
(
    id          int auto_increment
        primary key,
    name        varchar(100)                         null,
    image       varchar(50)                          null,
    category    varchar(50)                          null,
    description longtext                             null,
    price       double                               null,
    isEnabled   tinyint(1) default 1                 null,
    createdAt   datetime   default CURRENT_TIMESTAMP null,
    constraint products_pk_2
        unique (id)
);

create table orders
(
    id          int auto_increment
        primary key,
    userId      int      null,
    cartId      int      null,
    totalAmount double   null,
    createdAt   datetime null,
    constraint orders_pk_2
        unique (id)
);

create table carts
(
    id          int auto_increment
        primary key,
    userId      int                                null,
    isActive    tinyint(1)                         null,
    createdAt   datetime default CURRENT_TIMESTAMP null,
    totalAmount double                             null,
    constraint carts_pk_2
        unique (id)
);

create table cart_items
(
    id        int auto_increment
        primary key,
    cartId    int      null,
    productId int      null,
    quantity  int      null,
    updatedAt datetime null,
    createdAt datetime null,
    constraint cart_items_pk_2
        unique (id)
);