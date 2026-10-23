const bookstore = db.getSiblingDB("bookstore");

const integer = ["int", "long"];

const collections = [
  {
    name: "publishers",
    required: ["publisher_id", "name"],
    fields: {
      publisher_id: integer,
      name: "string",
      website: "string",
      contact_email: "string"
    },
    indexes: [
      [{ publisher_id: 1 }, { unique: true }],
      [{ name: 1 }, { unique: true }]
    ]
  },
  {
    name: "categories",
    required: ["category_id", "name"],
    fields: { category_id: integer, name: "string" },
    indexes: [
      [{ category_id: 1 }, { unique: true }],
      [{ name: 1 }, { unique: true }]
    ]
  },
  {
    name: "series",
    required: ["series_id", "name"],
    fields: { series_id: integer, name: "string", description: "string" },
    indexes: [[{ series_id: 1 }, { unique: true }]]
  },
  {
    name: "people",
    required: ["person_id", "full_name"],
    fields: {
      person_id: integer,
      full_name: "string",
      nationality: "string",
      birth_year: integer,
      death_year: integer,
      biography: "string",
      photo_url: "string"
    },
    indexes: [[{ person_id: 1 }, { unique: true }]]
  },
  {
    name: "books",
    required: ["book_id", "title"],
    fields: {
      book_id: integer,
      sku: "string",
      title: "string",
      category_id: integer,
      series_id: integer,
      publisher_id: integer,
      publish_date: "date",
      language: "string",
      sale_price: integer,
      stock_qty: integer,
      status: "string",
      short_description: "string",
      full_description: "string",
      table_of_contents: "string",
      age_group: "string"
    },
    indexes: [
      [{ book_id: 1 }, { unique: true }],
      [{ sku: 1 }, { unique: true, partialFilterExpression: { sku: { $type: "string" } } }],
      [{ category_id: 1 }, {}],
      [{ series_id: 1 }, {}],
      [{ publisher_id: 1 }, {}]
    ]
  },
  {
    name: "book_people",
    required: ["book_id", "person_id", "role"],
    fields: { book_id: integer, person_id: integer, role: "string" },
    indexes: [
      [{ book_id: 1, person_id: 1, role: 1 }, { unique: true }],
      [{ person_id: 1 }, {}]
    ]
  },
  {
    name: "book_images",
    required: ["image_id", "book_id", "url"],
    fields: { image_id: integer, book_id: integer, url: "string", sort_order: integer },
    indexes: [
      [{ image_id: 1 }, { unique: true }],
      [{ book_id: 1, sort_order: 1 }, {}]
    ]
  },
  {
    name: "tags",
    required: ["tag_id", "name"],
    fields: { tag_id: integer, name: "string" },
    indexes: [
      [{ tag_id: 1 }, { unique: true }],
      [{ name: 1 }, { unique: true }]
    ]
  },
  {
    name: "book_tags",
    required: ["book_id", "tag_id"],
    fields: { book_id: integer, tag_id: integer },
    indexes: [
      [{ book_id: 1, tag_id: 1 }, { unique: true }],
      [{ tag_id: 1 }, {}]
    ]
  },
  {
    name: "accounts",
    required: ["account_id", "full_name", "email", "password_hash", "role", "status"],
    fields: {
      account_id: integer,
      full_name: "string",
      email: "string",
      password_hash: "string",
      phone: "string",
      avatar_url: "string",
      role: { bsonType: "string", enum: ["ADMIN", "STAFF", "CUSTOMER"] },
      status: "string",
      created_at: "date",
      updated_at: "date"
    },
    indexes: [
      [{ account_id: 1 }, { unique: true }],
      [{ email: 1 }, { unique: true }]
    ]
  },
  {
    name: "addresses",
    required: ["address_id", "account_id"],
    fields: {
      address_id: integer,
      account_id: integer,
      recipient: "string",
      phone: "string",
      street: "string",
      ward: "string",
      district: "string",
      city: "string"
    },
    indexes: [
      [{ address_id: 1 }, { unique: true }],
      [{ account_id: 1 }, {}]
    ]
  },
  {
    name: "reviews",
    required: ["review_id", "book_id", "account_id"],
    fields: {
      review_id: integer,
      book_id: integer,
      account_id: integer,
      rating: integer,
      comment: "string",
      created_at: "date"
    },
    indexes: [
      [{ review_id: 1 }, { unique: true }],
      [{ book_id: 1 }, {}],
      [{ account_id: 1 }, {}]
    ]
  },
  {
    name: "orders",
    required: ["order_id", "account_id"],
    fields: {
      order_id: integer,
      account_id: integer,
      address_id: integer,
      status: "string",
      shipping_fee: integer,
      payment_method: "string",
      payment_status: "string",
      created_at: "date"
    },
    indexes: [
      [{ order_id: 1 }, { unique: true }],
      [{ account_id: 1, created_at: -1 }, {}],
      [{ address_id: 1 }, {}]
    ]
  },
  {
    name: "order_items",
    required: ["order_id", "book_id", "quantity", "unit_price"],
    fields: { order_id: integer, book_id: integer, quantity: integer, unit_price: integer },
    indexes: [
      [{ order_id: 1, book_id: 1 }, { unique: true }],
      [{ book_id: 1 }, {}]
    ]
  },
  {
    name: "roles",
    required: ["role_id", "role_name"],
    fields: { role_id: integer, role_name: "string", description: "string" },
    indexes: [
      [{ role_id: 1 }, { unique: true }],
      [{ role_name: 1 }, { unique: true }]
    ]
  },
  {
    name: "staff_roles",
    required: ["account_id", "role_id"],
    fields: { account_id: integer, role_id: integer },
    indexes: [
      [{ account_id: 1, role_id: 1 }, { unique: true }],
      [{ role_id: 1 }, {}]
    ]
  },
  {
    name: "permissions",
    required: ["permission_id", "permission_code"],
    fields: { permission_id: integer, permission_code: "string", description: "string" },
    indexes: [
      [{ permission_id: 1 }, { unique: true }],
      [{ permission_code: 1 }, { unique: true }]
    ]
  },
  {
    name: "role_permissions",
    required: ["role_id", "permission_id"],
    fields: { role_id: integer, permission_id: integer },
    indexes: [
      [{ role_id: 1, permission_id: 1 }, { unique: true }],
      [{ permission_id: 1 }, {}]
    ]
  },
  {
    name: "audit_logs",
    required: ["log_id", "action"],
    fields: {
      log_id: integer,
      account_id: integer,
      action: "string",
      entity_type: "string",
      entity_id: integer,
      details: "string",
      created_at: "date"
    },
    indexes: [
      [{ log_id: 1 }, { unique: true }],
      [{ account_id: 1, created_at: -1 }, {}],
      [{ entity_type: 1, entity_id: 1 }, {}]
    ]
  },
  {
    name: "inventory_transactions",
    required: ["transaction_id", "book_id", "quantity_change"],
    fields: {
      transaction_id: integer,
      book_id: integer,
      account_id: integer,
      quantity_change: integer,
      reason: "string",
      created_at: "date"
    },
    indexes: [
      [{ transaction_id: 1 }, { unique: true }],
      [{ book_id: 1, created_at: -1 }, {}],
      [{ account_id: 1, created_at: -1 }, {}]
    ]
  },
  {
    name: "carts",
    required: ["cart_id", "account_id"],
    fields: { cart_id: integer, account_id: integer, created_at: "date", updated_at: "date" },
    indexes: [
      [{ cart_id: 1 }, { unique: true }],
      [{ account_id: 1 }, { unique: true }]
    ]
  },
  {
    name: "cart_items",
    required: ["cart_id", "book_id", "quantity"],
    fields: { cart_id: integer, book_id: integer, quantity: integer, added_at: "date" },
    indexes: [
      [{ cart_id: 1, book_id: 1 }, { unique: true }],
      [{ book_id: 1 }, {}]
    ]
  }
];

function schemaFor(collection) {
  const properties = {};
  for (const [field, definition] of Object.entries(collection.fields)) {
    const fieldSchema = typeof definition === "string" ? { bsonType: definition } : definition;
    if (!collection.required.includes(field)) {
      const types = Array.isArray(fieldSchema.bsonType) ? fieldSchema.bsonType : [fieldSchema.bsonType];
      fieldSchema.bsonType = types.concat("null");
    }
    properties[field] = fieldSchema;
  }

  return { bsonType: "object", required: collection.required, properties };
}

for (const collection of collections) {
  bookstore.createCollection(collection.name, {
    validator: { $jsonSchema: schemaFor(collection) },
    validationLevel: "strict",
    validationAction: "error"
  });

  const target = bookstore.getCollection(collection.name);
  for (const [key, options] of collection.indexes) {
    target.createIndex(key, options);
  }
}
