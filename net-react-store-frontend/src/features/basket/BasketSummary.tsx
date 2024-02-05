import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
} from "@mui/material";
import { useEffect, useState } from "react";
import { currencyFormat } from "../../app/util/util";
import { useAppSelector } from "../../app/store/configureStore";

interface BasketSummaryProps {
  subTotal?: number;
}

const BasketSummary = ({ subTotal }: BasketSummaryProps) => {
  const { basket } = useAppSelector((state) => state.basket);
  if (subTotal === undefined)
    subTotal =
      basket?.items.reduce(
        (sum, item) => sum + item.price * item.quantity,
        0,
      ) ?? 0;
  const [deliveryFee, setDeliveryFee] = useState(0);

  useEffect(() => {
    const subTotal = basket?.items.reduce(
      (tot, item) => tot + item.price * item.quantity,
      0,
    );
    setDeliveryFee(subTotal && subTotal > 10000 ? 0 : 500);
  }, [basket]);

  return (
    <>
      <TableContainer component={Paper}>
        <Table>
          <TableBody>
            <TableRow>
              <TableCell colSpan={2}>Subtotal</TableCell>
              <TableCell align="right">${currencyFormat(subTotal)}</TableCell>
            </TableRow>
            <TableRow>
              <TableCell colSpan={2}>Delivery Fee</TableCell>
              <TableCell align="right">
                ${currencyFormat(deliveryFee)}
              </TableCell>
            </TableRow>
            <TableRow>
              <TableCell colSpan={2}>Total</TableCell>
              <TableCell align="right">
                ${currencyFormat(subTotal + deliveryFee)}
              </TableCell>
            </TableRow>
            <TableRow>
              <TableCell colSpan={2}>
                <span style={{ fontStyle: "italic" }}>
                  + Orders over $100 qualify for free delivery
                </span>
              </TableCell>
            </TableRow>
          </TableBody>
        </Table>
      </TableContainer>
    </>
  );
};

export default BasketSummary;
