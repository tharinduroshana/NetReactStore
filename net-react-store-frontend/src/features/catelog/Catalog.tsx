import React, { useEffect, useState } from "react";
import { Product } from "../../app/models/products";
import ProductList from "./ProductList";

const Catalog = () => {
  const [products, setProducts] = useState<Array<Product>>([]);

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    const response = await fetch("http://localhost:5198/api/products");
    const results = await response.json();
    setProducts(results);
  };

  return (
    <>
      <ProductList products={products} />
    </>
  );
};

export default Catalog;
