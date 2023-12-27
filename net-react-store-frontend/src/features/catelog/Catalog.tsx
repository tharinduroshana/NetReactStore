import React, { useEffect, useState } from "react";
import { Product } from "../../app/models/products";
import ProductList from "./ProductList";
import agent from "../../app/api/agent";

const Catalog = () => {
  const [products, setProducts] = useState<Array<Product>>([]);

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    agent.Catalog.list().then((products) => setProducts(products));
  };

  return (
    <>
      <ProductList products={products} />
    </>
  );
};

export default Catalog;
