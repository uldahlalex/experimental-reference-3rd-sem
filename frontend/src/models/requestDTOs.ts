
export interface GetBooksParams {
  startAt: number,
  pageSize: number,
  orderBy: string,
  bookSearchTerm: string
}

export interface GetAuthorsParams {
  startAt: number,
  pageSize: number,
  orderBy: string,
  authorSearchTerm: string
}

export interface AuthRequestDto {
  email: string,
  password: string
}


