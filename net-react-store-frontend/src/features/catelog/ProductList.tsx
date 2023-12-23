import { Product } from "../../app/models/products";
import { List } from "@mui/material";
import React from "react";
import ProductCard from "./ProductCard";

type ProductListProps = {
  products: Product[];
};

const ProductList = ({ products }: ProductListProps) => {
  return (
    <>
      <List>
        {products.map((item: Product) => (
          <ProductCard product={item} />
        ))}
      </List>
    </>
  );
};

export default ProductList;
