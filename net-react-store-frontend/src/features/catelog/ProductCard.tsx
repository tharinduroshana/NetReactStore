import { Product } from "../../app/models/products";
import { Avatar, ListItem, ListItemAvatar, ListItemText } from "@mui/material";
import React from "react";

type ProductCardType = {
  product: Product;
};

const ProductCard = ({ product }: ProductCardType) => {
  return (
    <>
      <ListItem key={product.id}>
        <ListItemAvatar>
          <Avatar src={product.pictureUrl} />
        </ListItemAvatar>
        <ListItemText>
          {product.name} - {product.price}
        </ListItemText>
      </ListItem>
    </>
  );
};

export default ProductCard;
