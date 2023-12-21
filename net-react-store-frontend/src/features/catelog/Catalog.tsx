import React from "react";
import { Product } from "../../app/models/products";
import { Typography } from "@mui/material";

type CatalogProps = {
  products: Product[];
  createProduct: () => void;
};

const Catalog = ({ products, createProduct }: CatalogProps) => {
  return (
    <div>
      <Typography variant="h1">Test</Typography>
      <div>
        {products.map((item: Product) => (
          <div key={item.id}>{item.name}</div>
        ))}
      </div>
      <button onClick={createProduct}>Test</button>
    </div>
  );
};

export default Catalog;
