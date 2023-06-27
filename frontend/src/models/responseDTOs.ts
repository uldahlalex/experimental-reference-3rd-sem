import {BookFeedItem, DiscoverFeed, ReadingListItem} from "./stateModels";


export interface BaseResponseDTO {
  messageToClient: string;

}
export interface GetBooksForFeedDTO extends BaseResponseDTO {
  responseData: BookFeedItem[]; //change to view model different from getReadingListDTO
}
export interface GetReadingListDTO extends BaseResponseDTO {
   responseData: ReadingListItem[]; //change to view model different from getBooksForFeedDTO
}
export interface GetBookById extends BaseResponseDTO {
  responseData: BookFeedItem;
}
export interface AuthResponseDTO extends BaseResponseDTO {
  responseData: string; //jwt
}
export interface DiscoverBooksDTO extends BaseResponseDTO {
  responseData: DiscoverFeed;
}
