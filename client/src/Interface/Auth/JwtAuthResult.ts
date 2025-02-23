export interface RefreshToken {
    userName: string;
    tokenString: string;
    expireAt: Date;
  }
  
  export interface JwtAuthResult {
    accessToken: string;
    refreshToken: RefreshToken;
  }