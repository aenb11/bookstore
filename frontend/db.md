// ===================== ENUMS =====================
// All enums are stored in the database as strings [varchar(20)] with constraint checks.

Enum account_role {
  Admin
  Staff
  Customer
}

Enum account_status {
  Active
  Suspended
}

Enum book_cover {
  Hardcover
  Paperback
}

Enum book_status {
  Draft
  Available
  Archived
  Discontinued
}

Enum book_age_group {
  Everyone
  Children
  YoungAdult
  Adult
}

Enum book_author_role {
  Author
  Translator
  Illustrator
  Editor
}

Enum order_status {
  Pending
  Confirmed
  Shipping
  Delivered
  Cancelled
}

Enum payment_method {
  CashOnDelivery
  CreditCard
}

Enum payment_status {
  Pending
  Paid
  Failed
  Refunded
}

Enum inventory_reason {
  PurchaseIn
  Sale
  OrderCancelled
  Adjustment
  Damaged
}

// ===================== TABLES =====================

Table accounts {
  id integer [pk, increment]
  clerk_id varchar(50) [not null, unique]
  email varchar(100) [not null, unique]
  full_name varchar(100) [not null]
  avatar_key varchar(255)
  role account_role [not null, default: account_role.Customer, note: "Saved as string [varchar(20)] + check constraint"]
  status account_status [not null, default: account_status.Active, note: "Saved as string [varchar(20)] + check constraint"]

  created_at timestamptz [not null, default: "now()"]
  updated_at timestamptz
  is_deleted boolean [not null, default: false]
  deleted_at timestamptz
}

Table addresses {
  id integer [pk, increment]
  account_id integer [not null]
  is_default boolean [not null, default: false]
  recipient_name varchar(100) [not null]
  recipient_phone varchar(10) [not null]
  street varchar(200) [not null]
  ward varchar(100) [not null]
  district varchar(100) [not null]
  city varchar(100) [not null]

  created_at timestamptz [not null, default: "now()"]
  updated_at timestamptz

  indexes {
    (account_id, is_default) [unique, note: "is_default = true, to ensure 1 account only has 1 default address"]
  }
}

Table audit_logs {
  id integer [pk, increment]
  account_id integer
  action varchar(50) [not null]
  entity_name varchar(100) [not null]
  entity_id integer
  details jsonb 
  created_at timestamptz [not null, default: "now()"]

  indexes {
    (entity_name, entity_id)
    account_id
    created_at
  }
}

Table authors {
  id integer [pk, increment]
  full_name varchar(100) [not null]
  nationality varchar(2) [note: "ISO 3166-1 alpha-2"]
  birth_year integer
  death_year integer [note: "Check: death_year IS NULL OR birth_year IS NULL OR death_year >= birth_year"]
  biography varchar(2000)
  photo_key varchar(255)

  created_at timestamptz [not null, default: "now()"]
  updated_at timestamptz
  is_deleted boolean [not null, default: false]
  deleted_at timestamptz
}

Table book_authors {
  book_id integer [not null]
  author_id integer [not null]
  role book_author_role [not null, note: "Saved as string [varchar(20)] + check constraint"]

  indexes {
    (book_id, author_id, role) [pk]
  }
}

Table book_images {
  id integer [pk, increment]
  book_id integer [not null]
  image_key varchar(255) [not null]
  sort_order integer [not null, note: "check constraint >= 0"]
}

Table book_tags {
  book_id integer [not null]
  tag_id integer [not null]

  indexes {
    (book_id, tag_id) [pk]
  }
}

Table books {
  id integer [pk, increment]
  isbn varchar(13) [note: "Check: isbn IS NULL OR LENGTH(isbn) = 13"]
  title varchar(200) [not null]
  category_id integer [not null]
  serie_id integer
  publisher_id integer [not null]
  publication_year integer [not null, note: "Check: >= 0"]
  language varchar(2) [not null, note: "ISO 639-1"]
  page_count integer [not null, note: "Check: >= 0"]
  cover_type book_cover [not null, note: "Saved as string [varchar(20)] + check constraint"]
  weight integer [not null, note: "Gram. Check: >= 0"]
  age_group book_age_group [not null, note: "Saved as string [varchar(20)] + check constraint"]
  list_price integer [not null, note: "VND. Check: ck_books_prices (list_price >= 0 AND sale_price >= 0 AND sale_price <= list_price)"]
  sale_price integer [not null]
  stock_quantity integer [not null, default: 0, note: "Check: >= 0"]
  status book_status [not null, default: book_status.Available, note: "Saved as string [varchar(20)] + check constraint"]
  description varchar(1000) [not null]

  created_at timestamptz [not null, default: "now()"]
  updated_at timestamptz
  is_deleted boolean [not null, default: false]
  deleted_at timestamptz

  indexes {
    isbn [unique]
  }
}

Table cart_items {
  account_id integer [not null]
  book_id integer [not null]
  quantity integer [not null, default: 1, note: "Check: quantity > 0"]
  created_at timestamptz [not null, default: "now()"]

  indexes {
    (account_id, book_id) [pk]
  }
}

Table categories {
  id integer [pk, increment]
  name varchar(100) [not null, unique]

  created_at timestamptz [not null, default: "now()"]
  updated_at timestamptz
  is_deleted boolean [not null, default: false]
  deleted_at timestamptz
}

Table inventory_transactions {
  id integer [pk, increment]
  book_id integer [not null]
  order_id integer
  account_id integer
  quantity_change integer [not null, note: "Check: quantity_change <> 0"]
  balance_after integer [not null, note: "Check: >= 0"]
  reason inventory_reason [not null, note: "Saved as string [varchar(20)] + check constraint"]
  note varchar(500)
  created_at timestamptz [not null, default: "now()"]

  indexes {
    (book_id, created_at)
  }
}

Table order_items {
  order_id integer [not null]
  book_id integer [not null]
  quantity integer [not null, note: "Check: quantity > 0"]
  price integer [not null, note: "Check: >= 0"]

  indexes {
    (order_id, book_id) [pk]
  }
}

Table orders {
  id integer [pk, increment]
  account_id integer [not null]

  recipient_name varchar(100) [not null]
  recipient_phone varchar(10) [not null]
  street varchar(200) [not null]
  ward varchar(100) [not null]
  district varchar(100) [not null]
  city varchar(100) [not null]

  shipping_fee integer [not null, note: "Check: >= 0"]
  status order_status [not null, default: order_status.Pending, note: "Saved as string [varchar(20)] + check constraint"]
  payment_method payment_method [not null, default: payment_method.CashOnDelivery, note: "Saved as string [varchar(20)] + check constraint"]
  payment_status payment_status [not null, default: payment_status.Pending, note: "Saved as string [varchar(20)] + check constraint"]

  created_at timestamptz [not null, default: "now()"]
  updated_at timestamptz

  indexes {
    (account_id, created_at) 
  }
}

Table publishers {
  id integer [pk, increment]
  name varchar(100) [not null, unique]
  website varchar(200)
  contact_email varchar(100)

  created_at timestamptz [not null, default: "now()"]
  updated_at timestamptz
  is_deleted boolean [not null, default: false]
  deleted_at timestamptz
}

Table reviews {
  id integer [pk, increment]
  book_id integer [not null]
  account_id integer [not null]
  rating integer [not null, note: "Check: rating BETWEEN 1 AND 5"]
  comment varchar(1000)

  created_at timestamptz [not null, default: "now()"]
  updated_at timestamptz
  is_deleted boolean [not null, default: false]
  deleted_at timestamptz

  indexes {
    (account_id, book_id) [unique]
  }
}

Table series {
  id integer [pk, increment]
  name varchar(100) [not null]
  description varchar(500)

  created_at timestamptz [not null, default: "now()"]
  updated_at timestamptz
  is_deleted boolean [not null, default: false]
  deleted_at timestamptz

  indexes {
    name [unique]
  }
}

Table tags {
  id integer [pk, increment]
  name varchar(100) [not null]

  created_at timestamptz [not null, default: "now()"]
  updated_at timestamptz
  is_deleted boolean [not null, default: false]
  deleted_at timestamptz

  indexes {
    name [unique]
  }
}

// ===================== REFERENCES =====================

Ref: addresses.account_id > accounts.id [delete: cascade]
Ref: audit_logs.account_id >? accounts.id [delete: restrict]

Ref: book_authors.book_id > books.id [delete: cascade]
Ref: book_authors.author_id > authors.id [delete: cascade]
Ref: book_images.book_id > books.id [delete: cascade]
Ref: book_tags.book_id > books.id [delete: cascade]
Ref: book_tags.tag_id > tags.id [delete: cascade]

Ref: books.category_id > categories.id [delete: restrict]
Ref: books.publisher_id > publishers.id [delete: restrict]
Ref: books.serie_id >? series.id [delete: restrict]

Ref: cart_items.account_id > accounts.id [delete: cascade]
Ref: cart_items.book_id > books.id [delete: cascade]

Ref: inventory_transactions.account_id >? accounts.id [delete: restrict]
Ref: inventory_transactions.book_id > books.id [delete: restrict]
Ref: inventory_transactions.order_id >? orders.id [delete: restrict]

Ref: order_items.order_id > orders.id [delete: cascade]
Ref: order_items.book_id > books.id [delete: restrict]
Ref: orders.account_id > accounts.id [delete: restrict]

Ref: reviews.account_id > accounts.id [delete: restrict]
Ref: reviews.book_id > books.id [delete: cascade]

