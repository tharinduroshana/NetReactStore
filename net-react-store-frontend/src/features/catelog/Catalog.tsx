import React, { useEffect, useState } from "react";
import { Product } from "../../app/models/products";
import ProductList from "./ProductList";
import agent from "../../app/api/agent";
import LoadingComponent from "../../app/layout/LoadingComponent";

const Catalog = () => {
  const [products, setProducts] = useState<Array<Product>>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    agent.Catalog.list()
      .then((products) => setProducts(products))
      .catch((error) => console.log(error))
      .finally(() => setLoading(false));
  };

  if (loading) {
    return <LoadingComponent message="Loading catalog" />;
  }

  return (
    <>
      <ProductList products={products} />
    </>
  );
};

export default Catalog;
