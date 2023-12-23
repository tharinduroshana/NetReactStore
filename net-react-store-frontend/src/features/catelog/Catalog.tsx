import React from "react";
import { Product } from "../../app/models/products";
import { Button } from "@mui/material";
import ProductList from "./ProductList";

type CatalogProps = {
  products: Product[];
  createProduct: () => void;
};

const Catalog = ({ products, createProduct }: CatalogProps) => {
  console.log(products);
  return (
    <>
      <ProductList products={products} />
      <Button variant="contained" onClick={createProduct}>
        Test
      </Button>
    </>
  );
};

export default Catalog;
