import { Box, Pagination, Typography } from "@mui/material";
import React from "react";
import { MetaData } from "../models/pagination";

type AppPaginationProps = {
  metaData: MetaData;
  onPageChange: (page: number) => void;
};

const AppPagination = ({ metaData, onPageChange }: AppPaginationProps) => {
  const { currentPage, totalPages, totalCount, pageSize } = metaData;
  return (
    <Box display="flex" justifyContent="space-between" alignItems="center">
      <Typography>
        Displaying {(currentPage - 1) * 6 + 1} -{" "}
        {currentPage * pageSize > totalCount
          ? totalCount
          : currentPage * pageSize}{" "}
        of {totalCount} items
      </Typography>
      <Pagination
        color="secondary"
        size="large"
        count={totalPages}
        page={currentPage}
        onChange={(event, page) => onPageChange(page)}
      />
    </Box>
  );
};

export default AppPagination;
