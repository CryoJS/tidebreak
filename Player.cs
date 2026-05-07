using System;
using System.Collections.Generic;
using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tidebreak;

class Player
{
    // Store possible player states
    private enum PlayerState
    {
        Idle,
        Running,
        Landing,
        Jumping,
        Falling,
        Swimming,
        Sliding,
        Latching,
        Climbing,
        Hanging,
        Dead
    }

    // Store player physics constants (pixels/seconds)
    private const float GRAVITY = 8000f;
    private const float MAX_SPEED = 700f;
    private const float JUMP_VELOCITY = -2500f;
    private const float MAX_FALL_SPEED = 3000f;
    private const float WATER_RESISTANCE = 1000f;
    private const float WALL_JUMP_VELOCITY = 50f;
    private const float ZIPLINE_SPEED = 700f;

    private const float RUN_THRESHOLD = 50f;
    private const float FALL_THRESHOLD = 0.9f * MAX_FALL_SPEED;
    private const float COYOTE_MAX = 0.1f; // seconds
    private const float SLIDE_MAX = 3f; // seconds

    // Store player animation constants
    private const int FRAME_DURATION = 100; // Duration of each animation frame in milliseconds
    private const int ANIM_SIZE = 128;      // Pixel size (player is in the center of a 128x128 frame)
    private const int PLAYER_WIDTH = 10;    // Actual width of the player sprite (for collisions)
    private const int PLAYER_HEIGHT = 30;   // Actual height of the player sprite (for collisions)
    private const int TILE_CHECK_SIZE = 2;  // Farthest # of tiles away from player to check for collisions

    // Store player movement data
    private PlayerState state = PlayerState.Idle;
    private PlayerState prevState = PlayerState.Idle;

    int moveInput;
    bool isGrounded = false;
    float coyoteTime = 0;

    bool isSliding = false;
    float slideTime = 0;

    private Rectangle rec = new Rectangle(0, 0, PLAYER_WIDTH * Game1.PIXEL_SCALE, PLAYER_HEIGHT * Game1.PIXEL_SCALE);
    public Vector2 pos = new Vector2(0, 0);
    private Vector2 vel = new Vector2(0, 0); // Per second
    SpriteEffects direction = Animation.FLIP_NONE;

    // Store player swim data // TODO
    bool inWater;
    int oxygen = 100;

    // Store player animation data
    private Texture2D idleImg;
    private Texture2D runImg;
    private Texture2D landImg;
    private Texture2D jumpImg;
    private Texture2D fallImg;
    private Texture2D swimImg;
    private Texture2D slideImg;
    private Texture2D wallJumpImg;
    private Texture2D climbImg;
    private Texture2D hangImg;
    private Texture2D deathImg;

    private Animation idleAnim;
    private Animation runAnim;
    private Animation landAnim;
    private Animation jumpAnim;
    private Animation fallAnim;
    private Animation swimAnim;
    private Animation slideAnim;
    private Animation wallJumpAnim;
    private Animation climbAnim;
    private Animation hangAnim;
    private Animation deathAnim;

    public Player(ContentManager content)
    {
        // Load player animation spreadsheets
        idleImg = content.Load<Texture2D>("Images/Sprites/Player/HeroIdle");
        runImg = content.Load<Texture2D>("Images/Sprites/Player/HeroRun");
        landImg = content.Load<Texture2D>("Images/Sprites/Player/HeroLand");
        jumpImg = content.Load<Texture2D>("Images/Sprites/Player/HeroJump");
        fallImg = content.Load<Texture2D>("Images/Sprites/Player/HeroFall");
        swimImg = content.Load<Texture2D>("Images/Sprites/Player/HeroSwim");
        slideImg = content.Load<Texture2D>("Images/Sprites/Player/HeroSlide");
        wallJumpImg = content.Load<Texture2D>("Images/Sprites/Player/HeroWallJump");
        climbImg = content.Load<Texture2D>("Images/Sprites/Player/HeroClimb");
        hangImg = content.Load<Texture2D>("Images/Sprites/Player/HeroHang");
        deathImg = content.Load<Texture2D>("Images/Sprites/Player/HeroDeath");

        // Create player animations
        idleAnim = new Animation(idleImg, 10, 1, 10, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 10 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, true, "PlayerIdle");
        runAnim = new Animation(runImg, 10, 1, 10, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 10 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, true, "PlayerRun");
        landAnim = new Animation(landImg, 5, 1, 5, 0, Animation.NO_IDLE, 1, 5 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, false, "PlayerLand");
        jumpAnim = new Animation(jumpImg, 6, 1, 6, 0, 5, 1, 6 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, false, "PlayerJump");
        fallAnim = new Animation(fallImg, 3, 1, 3, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 3 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, true, "PlayerFall");
        swimAnim = new Animation(swimImg, 6, 1, 6, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 6 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, true, "PlayerSwim");
        slideAnim = new Animation(slideImg, 8, 1, 8, 0, Animation.NO_IDLE, 1, 8 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, false, "PlayerSlide");
        wallJumpAnim = new Animation(wallJumpImg, 4, 1, 4, 0, Animation.NO_IDLE, 1, 4 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, false, "PlayerWallJump");
        climbAnim = new Animation(climbImg, 4, 1, 4, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 4 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, true, "PlayerClimb");
        hangAnim = new Animation(hangImg, 3, 1, 3, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 3 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, true, "PlayerHang");
        deathAnim = new Animation(deathImg, 23, 1, 23, 0, Animation.NO_IDLE, 1, 23 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, false, "PlayerDeath");
    }

    private Animation GetCurAnim()
    {
        switch (state)
        {
            case PlayerState.Idle:
                return idleAnim;

            case PlayerState.Running:
                return runAnim;

            case PlayerState.Landing:
                return landAnim;

            case PlayerState.Jumping:
                return jumpAnim;

            case PlayerState.Falling:
                return fallAnim;
            
            case PlayerState.Swimming:
                return swimAnim;

            case PlayerState.Sliding:
                return slideAnim;

            case PlayerState.Latching:
                return wallJumpAnim;

            case PlayerState.Climbing:
                return climbAnim;

            case PlayerState.Hanging:
                return hangAnim;

            case PlayerState.Dead:
                return deathAnim;

            default:
                return idleAnim;
        }
    }

    private void SetState(PlayerState newState)
    {
        if (state != newState)
        {
            prevState = state;
            state = newState;

            GetCurAnim().Activate(true);
        }
    }

    public void Update(GameTime gameTime, KeyboardState kb, KeyboardState prevKb, Cam2D camera, Map map)
    {
        // Move player, check collisions, update the player's rectangle and animation state
        Move(gameTime, kb, prevKb);
        UpdateRec();
        CheckCollisions(gameTime, map);
        UpdateRec();
        UpdateAnimations(gameTime);

        // Update camera to follow player
        camera.LookAt(rec);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        GetCurAnim().Draw(spriteBatch, Color.White, direction);
    }

    private void Move(GameTime gameTime, KeyboardState kb, KeyboardState prevKb)
    {
        // Move the player left or right based on player input
        moveInput = 0;

        if (kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.Left))
        {
            moveInput = -1;
        }
        else if (kb.IsKeyDown(Keys.D) || kb.IsKeyDown(Keys.Right))
        {
            moveInput = 1;
        }

        vel.X = moveInput * MAX_SPEED;

        // Change player movement depending on swimming or not
        if (inWater)
        {
            // Slow player down by water resistance
            vel.X = vel.X - Math.Sign(vel.X) * Math.Min(Math.Abs(vel.X), WATER_RESISTANCE * (float)gameTime.ElapsedGameTime.TotalSeconds);
            vel.Y = vel.Y - Math.Sign(vel.Y) * Math.Min(Math.Abs(vel.Y), WATER_RESISTANCE * (float)gameTime.ElapsedGameTime.TotalSeconds);

            // Swim up or swim down instead of jump/dive
            if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.Up))
            {
                vel.Y = -MAX_SPEED;
            }
            else if (kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.Down))
            {
                vel.Y = MAX_SPEED;
            }
        }
        else
        {
            // Update the player's speed by player's fall speed when airborne
            if (!isGrounded) vel.Y += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // If the player presses a jump key, and is on the grounded, update their speed to move up and play jump animation
            if (isGrounded && (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.Up) || kb.IsKeyDown(Keys.Space)))
            {
                // Since the player jumped, they are no longer grounded
                isGrounded = false;
                coyoteTime = -1;

                // Update the player's velocity, making them jump
                vel.Y = JUMP_VELOCITY;

                // Play jump animation
                SetState(PlayerState.Jumping);
            }

            // Set player as assumed to not be sliding
            isSliding = false;

            // Slide or airdive if the player presses the button to, depending on if the player is on the ground or not
            if (isGrounded)
            {
                // If the player is holding slide key while on ground, he is sliding
                if (kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.Down))
                {
                    isSliding = true;
                }
            }
            else if ((kb.IsKeyDown(Keys.S) && !prevKb.IsKeyDown(Keys.S)) || (kb.IsKeyDown(Keys.Down) && !prevKb.IsKeyDown(Keys.Down)))
            {
                // Update the player's velocity, making them air dive
                vel.Y = MAX_FALL_SPEED;
            }
        }

        // Clamp the speeds to make sure the player does not exceed their max speed
        vel.X = MathHelper.Clamp(vel.X, -MAX_SPEED, MAX_SPEED);
        vel.Y = MathHelper.Clamp(vel.Y, -MAX_FALL_SPEED, MAX_FALL_SPEED);

        // Update the player's position with velocity
        pos.X += vel.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
        pos.Y += vel.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    private void UpdateRec()
    {
        rec.X = (int)pos.X;
        rec.Y = (int)pos.Y;
    }

    private void CheckCollisions(GameTime gameTime, Map map)
    {
        // Set up player collision direction / body part collision rectangles
        Rectangle top = new Rectangle((int)(rec.X + 0.2 * rec.Width), rec.Y, (int)(0.6 * rec.Width), (int)(0.25 * rec.Height));
        Rectangle bottom = new Rectangle((int)(rec.X + 0.15 * rec.Width), (int)(rec.Y + 0.75 * rec.Height), (int)(0.7 * rec.Width), (int)(0.25 * rec.Height));
        Rectangle left = new Rectangle(rec.X, (int)(rec.Y + 0.05 * rec.Height), (int)(0.5 * rec.Width), (int)(0.9 * rec.Height));
        Rectangle right = new Rectangle((int)(rec.X + 0.5 * rec.Width), (int)(rec.Y + 0.05 * rec.Height), (int)(0.5 * rec.Width), (int)(0.9 * rec.Height));

        // Get the current tile of the map the player is on, and create variables for storing tile type and recs
        int tileX = rec.Center.X / (Game1.TILE_SIZE * Game1.PIXEL_SCALE);
        int tileY = rec.Center.Y / (Game1.TILE_SIZE * Game1.PIXEL_SCALE);

        int tileType;
        Rectangle tileRec;

        // Set the player to not be on the ground unless collision checks conclude otherwise
        isGrounded = false;

        // Check for platform collisions within a certain radius of the player
        for (int x = Math.Max(0, tileX - TILE_CHECK_SIZE); x <= Math.Min(map.sizeX - 1, tileX + TILE_CHECK_SIZE); x++)
        {
            for (int y = Math.Max(0, tileY - TILE_CHECK_SIZE); y <= Math.Min(map.sizeY - 1, tileY + TILE_CHECK_SIZE); y++)
            {
                tileType = map.tiles[x, y].type;
                tileRec = map.tiles[x, y].rec;

                // Check if any collision occurred with a collidable tile
                if (1 <= tileType && tileType <= Tile.PLATFORM_TYPE_AMOUNT && rec.Intersects(tileRec))
                {
                    // Check if player collides up or down
                    if (tileRec.Intersects(top))
                    {
                        // Set the player right below the platform, let upwards speed be blocked by the ceiling, letting gravity take over
                        pos.Y = tileRec.Bottom + 1;
                        vel.Y = 0;
                    }
                    else if (tileRec.Intersects(bottom))
                    {
                        // Set the player right above the platform, let downwards speed be blocked by the floor
                        pos.Y = tileRec.Top - rec.Height + 1;
                        vel.Y = 0;

                        // Update grounded status
                        isGrounded = true;
                    }

                    // Check if player collides left or right
                    if (tileRec.Intersects(left))
                    {
                        // Set the player right to the right of the platform, let leftwards speed be blocked by the floor
                        pos.X = tileRec.Right + 1;
                        vel.X = 0;
                    }
                    else if (tileRec.Intersects(right))
                    {
                        // Set the player right to the left of the platform, let rightwards speed be blocked by the floor
                        pos.X = tileRec.Left - rec.Width;
                        vel.X = 0;
                    }
                }
            }
        }

        // Perform tile collision checks with player (for the tile the player is at) (if tile exists)
        if (0 <= tileX && tileX < map.sizeX && 0 <= tileY && tileY < map.sizeY)
        {
            tileType = map.tiles[tileX, tileY].type;

            switch (tileType)
            {
                case Tile.WATER:
                    inWater = true;
                    break;
            }
        }

        // Let player be grounded for a short time frame even after not detecting ground collision
        if (isGrounded)
        {
            coyoteTime = COYOTE_MAX;
        }
        else if (coyoteTime > 0)
        {
            isGrounded = true;
            coyoteTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    private void UpdateAnimations(GameTime gameTime)
    {
        // Flip sprite based on player movement direction
        if (vel.X > 0)
        {
            direction = Animation.FLIP_NONE;
        }
        else if (vel.X < 0)
        {
            direction = Animation.FLIP_HORIZONTAL;
        }

        // If the player is in water, they are in the swimming animation no matter what
        if (inWater)
        {
            SetState(PlayerState.Swimming);
        }

        // Check if state needs to be changed
        if (!isGrounded)
        {
            // If airborne, check if fall or jump animation should be playing
            if (vel.Y < 0)
            {
                SetState(PlayerState.Jumping);
            }
            else if (vel.Y >= FALL_THRESHOLD)
            {
                SetState(PlayerState.Falling);
            }
        }
        else if ((state == PlayerState.Falling || state == PlayerState.Jumping) && moveInput == 0)
        {
            // Play landing animation as we just landed only if no input
            SetState(PlayerState.Landing);
        }
        else if (state == PlayerState.Landing)
        {
            // When the landing animation finishes, go to Idle
            if (!landAnim.IsAnimating())
            {
                SetState(PlayerState.Idle);
            }
        }
        else if (moveInput != 0)
        {
            // Player is trying to move
            SetState(PlayerState.Running);
        }
        else
        {
            // Otherwise idle
            SetState(PlayerState.Idle);
        }

        // Update the player's current animation
        GetCurAnim().TranslateTo(pos.X - (ANIM_SIZE * Game1.PIXEL_SCALE - rec.Width) / 2, pos.Y - (ANIM_SIZE * Game1.PIXEL_SCALE - rec.Height) / 2);
        GetCurAnim().Update(gameTime);
    }
}