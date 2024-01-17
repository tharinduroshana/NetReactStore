export interface Product {
  name: string;
  description: string;
  brand: string;
  pictureUrl: string;
  price: number;
  quantityInStock: number;
  type: string;
  id: number;
}

export interface ProductParams {
  orderBy: string;
  searchTerm?: string;
  types: string[];
  brands: string[];
  pageNumber: number;
  pageSize: number;
}
