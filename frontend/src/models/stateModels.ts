

export interface AccountProfileData {
  email?: string;
  role?: string;
  endUserId?: number;
  status?: string;
  pravatarId?: number;
}

export interface BookFeedItem {
  isOnMyReadingList?: boolean;
  title?: string;
  authors?: string[],
  bookId?: number,
  coverImgUrl?: string
}


export interface ReadingListItem {
  bookId: number;
  title: string;
  publisher: string;
  coverImgUrl: string;
}

export interface DiscoverFeed {
  recentlyAdded: BookFeedItem[];
  notOnReadingList: BookFeedItem[]
}



