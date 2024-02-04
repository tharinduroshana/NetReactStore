import { Order } from "../../app/models/order";
import { Box, Button } from "@mui/material";
import Typography from "@mui/material/Typography";
import BasketTable from "../basket/BasketTable";
import { BasketItem } from "../../app/models/basket";
import Grid from "@mui/material/Grid";
import BasketSummary from "../basket/BasketSummary";

interface OrderDetailsProps {
  order: Order;
  setSelectOrder: (id: number) => void;
}

const OrderDetails = ({ order, setSelectOrder }: OrderDetailsProps) => {
  const subTotal = order.orderItems.reduce(
    (sum, item) => sum + item.price * item.quantity,
    0,
  );
  return (
    <>
      <Box display="flex" justifyContent="space-between">
        <Typography sx={{ p: 2 }} gutterBottom variant="h4">
          Order #{order.id} - {order.orderStatus}
        </Typography>
        <Button
          onClick={() => setSelectOrder(0)}
          sx={{ m: 2 }}
          size="large"
          variant="contained"
        >
          Back To Orders
        </Button>
      </Box>
      <BasketTable items={order.orderItems as BasketItem[]} isBasket={false} />
      <Grid container>
        <Grid item xs={6} />
        <Grid item xs={6}>
          <BasketSummary subTotal={subTotal} />
        </Grid>
      </Grid>
    </>
  );
};

export default OrderDetails;
