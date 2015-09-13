using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AnimoNex.game;

namespace AnimoNex.types
{
    #region CharactersAndAnimation
    public struct character
    {
        public static character setTo(character c)
        {
            character ouput = new character();

            ouput.accuracy = c.accuracy;
            ouput.angle = c.angle;
            ouput.armor = c.armor;
            ouput.B_baseProps = c.B_baseProps;
            ouput.body = new block[0];
            ouput.body = new block[c.body.Length];
            for (int i = 0; i != c.body.Length; i++)
            {
                ouput.body[i] = c.body[i];
            }
            ouput.controlType = c.controlType;
            ouput.current_weap = c.current_weap;
            ouput.currentAnim = c.currentAnim;
            ouput.f_currentFrame = c.f_currentFrame;
            ouput.attrib = c.attrib;
            ouput.name = c.name;
            ouput.pointingRight = c.pointingRight;
            ouput.Primary_weap = c.Primary_weap;
            ouput.Secondary_weap = c.Secondary_weap;
            ouput.SideArm_weap = c.SideArm_weap;
            ouput.Type = c.Type;
            ouput.action = c.action;

            return ouput;
        }

        public string name;
        public string action;
        public string weaponInUse;
        public int special_tag;
        public ControlType controlType;
        public CharType Type;
        public inventory Inventory;
        public block[] body;
        public Animation currentAnim;
        public float f_currentFrame;
        public Base B_baseProps;
        public weapon Primary_weap, Secondary_weap, SideArm_weap, current_weap;
        public float angle;
        public bool pointingRight;
        public bool reordered;
        //stats
        public float armor;
        public skill attrib;
        public float accuracy;
        //AI
        public AInode brain;
        public character(string Name, Vector2 Position, ControlType CType, CharType type)
        {
            controlType = CType;
            action = "idle";
            special_tag = -1;
            Type = type;
            name = Name;
            B_baseProps = new Base(Position, 70f);
            f_currentFrame = 0;
            body = null;
            currentAnim = new Animation();
            angle = 0;
            accuracy = 1;
            pointingRight = true;
            attrib = new skill();
            armor = 0;
            Primary_weap = Secondary_weap = SideArm_weap = current_weap = new weapon();
            Inventory = new inventory(5, null);
            weaponInUse = "unarmed";
            brain = new AInode(false);
            reordered = false;
        }
        public character(string Name, Vector2 Position, skill Skill, ControlType CType, CharType type)
        {
            controlType = CType;
            action = "idle";
            special_tag = -1;
            Type = type;
            name = Name;
            B_baseProps = new Base(Position, 70f);
            f_currentFrame = 0;
            body = null;
            currentAnim = new Animation();
            angle = 0;
            accuracy = 1;
            attrib = Skill;
            armor = 0;
            pointingRight = true;
            Primary_weap = Secondary_weap = SideArm_weap = current_weap = new weapon();
            Inventory = new inventory(5, null);
            weaponInUse = "unarmed";
            brain = new AInode(false);
            reordered = false;
        }
    }
    public struct skill
    {
        public int hp_max, sta_max, stl_max, med_max, mech_max, str_max;
        public int kills;
        public int score;
        public float hp_current, sta_current, stl_current, hun_current, med_current, mech_current, str_current;
        public float sta_prog, stl_prog, med_prog, mech_prog, str_prog;
        public skill(int hp)
        {
            hp_max = hp;
            sta_max = stl_max = med_max = mech_max = str_max = 1;

            hp_current = hp_max;
            sta_current = sta_max;
            stl_current = stl_max;
            med_current = med_max;
            mech_current = mech_max;
            str_current = str_max;
            hun_current = 0;
            kills = 0;
            score = 0;
            sta_prog = stl_prog = med_prog = mech_prog = str_prog = 0;
        }
    }
    public struct block
    {
        public string name;
        public string[] TexturePath; //for the editor only
        public Texture2D[] texture;
        public Vector2[] v_collison_map;
        public Vector2[] v_collison_mapOR;
        public int maxHP, currentHP;
        public int damageLevel, zorder;
        public Vector2 position;
        public Vector2 pivot;
        public int[] children, parents;
        public int width, height;
        public float angle;
        public block(string Name, Texture2D Texture, Vector2 Position)
        {
            TexturePath = new string[1];
            texture = new Texture2D[1];
            position = Position;
            v_collison_map = gfunc.collision.MakeCollisionMap(Vector2.Zero, Texture, gfunc.i_collision_accuracy);
            v_collison_mapOR = gfunc.collision.MakeCollisionMap(Vector2.One, Texture, gfunc.i_collision_accuracy);
            //V_FUCKING_ORIG = null;
            texture[0] = Texture;
            TexturePath[0] = "";
            name = Name;
            pivot = Vector2.Zero;
            children = parents = new int[0];
            width = texture[0].Width;
            height = texture[0].Height;
            maxHP = currentHP = 100;
            angle = 0;
            zorder = damageLevel = 0;
        }
        public block(string Name, Texture2D Texture, Vector2 Position, Vector2 Pivot)
        {
            TexturePath = new string[1];
            texture = new Texture2D[1];

            texture[0] = Texture;
            TexturePath[0] = "";
            position = Position;
            v_collison_map = gfunc.collision.MakeCollisionMap(Vector2.Zero, Texture, gfunc.i_collision_accuracy);
            v_collison_mapOR = gfunc.collision.MakeCollisionMap(Vector2.One, Texture, gfunc.i_collision_accuracy);
            name = Name;
            pivot = Pivot;
            children = parents = null;
            maxHP = currentHP = 100;
            width = texture[0].Width;
            height = texture[0].Height;
            angle = 0;
            zorder = damageLevel = 0;
        }
        public block(string Name, Texture2D Texture, Vector2 Position, Vector2 Pivot, int Width, int Height, float Angle)
        {
            TexturePath = new string[1];
            texture = new Texture2D[1];
            position = Position;
            v_collison_map = gfunc.collision.MakeCollisionMap(Vector2.Zero, Texture, gfunc.i_collision_accuracy);
            v_collison_mapOR = gfunc.collision.MakeCollisionMap(Vector2.One, Texture, gfunc.i_collision_accuracy);
            texture[0] = Texture;
            TexturePath[0] = "";
            name = Name;
            pivot = Pivot;
            children = parents = null;
            maxHP = currentHP = 30;
            width = Width;
            height = Height;
            angle = Angle;
            zorder = damageLevel = 0;
        }
    }
    public struct Animation
    {
        public string name;
        public frame[] f_frame;

        public Animation(string Name, frame[] frames)
        {
            name = Name;
            f_frame = frames;
        }
    }
    public struct frame
    {
        public float[] angle;
        public Vector2[] position;
        public bool key_frame;
        public frame(float[] angles, Vector2[] Position, bool isKeyframe)
        {
            angle = angles;
            position = Position;
            key_frame = isKeyframe;
        }
    }
    public enum ControlType
    {
        player_current = 0, player_network = 1, NPC = 3
    };
    public enum CharType
    {
        soldier, survivor, bandit, zombie
    };
    public enum LastName
    {

        Smith = 0,
        Johnson = 1,
        Williams = 2,
        Jones = 3,
        Brown = 4,
        Davis = 5,
        Miller = 6,
        Wilson = 7,
        Moore = 8,
        Taylor = 9,
        Anderson = 10,
        Thomas = 11,
        White = 12,
        Harris = 13,
        Thompson = 14,
        Garcia = 15,
        Martinez = 16,
        Robinson = 17,
        Lee = 18,
        Walker = 19,
        Hall = 20,
        Young = 21,
        King = 22,
        Edwards = 23,
        Collins = 24,
        Reed = 25,
        Cox = 26,
        Freeman = 27,
        Simpson = 28,
        Black = 29,
        McCoy = 30,
        Griffith = 31,
        Newton = 32,
        McBryde = 33,
        Underwood = 34,
        Collier = 35,
        Wolf = 36,
        Kirk = 37,
        Bond = 38,
        Snow = 39,
        Meyers = 40,
        Valentine = 41,
        Maddox = 42,
        Crosby = 43,
        Chan = 44,
        Comdure = 45,
        Canning = 46,
        Billock = 47,
        Theisen = 48,
        Blair = 49,
        Roerig = 50,
        McVey = 51
    };
    public enum FirstNameM
    {
        Jacob = 0,
        Nicolas = 1,
        Michael = 2,
        Ethan = 3,
        Joshua = 4,
        Daniel = 5,
        Alex = 6,
        Anthony = 7,
        William = 8,
        Chistopher = 9,
        Matthew = 10,
        Ryan = 11,
        Justin = 12,
        Brandon = 13,
        Tyler = 14,
        John = 15,
        Kyle = 16,
        Zach = 17,
        Kevin = 18,
        Cody = 19,
        Eric = 20,
        Steve = 21,
        Tom = 22,
        Jordan = 23,
        Brian = 24,
        Aaron = 25,
        Tim = 26,
        Ben = 27,
        Rich = 28,
        Dick = 29,
        Adam = 30,
        Jose = 31,
        Nate = 32,
        Jeremy = 33,
        Travis = 34,
        Jeff = 35,
        Dylan = 36,
        Mark = 37,
        Jason = 38,
        Kirk = 39,
        Alec = 40,
        Scott = 41,
        James = 42,
        George = 43,
        Rob = 44,
        Frank = 45,
        Edward = 46,
        Hernry = 47,
        Nolan = 48,
        Chase = 49,
        Derrick = 50,
        Gordan = 51,
    };
    #endregion
    #region LevelsAndProps
    public struct prop
    {
        public Texture2D texture;
        public string texture_path;
        public string name;
        public int damageLevel, HP, X, Y;
        public float angle, scale;
        public prop(Texture2D Texture, string Name, int hp, float ang, float Scale)
        {
            texture = Texture;
            texture_path = "";
            name = Name;
            damageLevel = X = Y = 0;
            HP = hp;
            angle = ang;
            scale = Scale;
        }

    }
    public struct region
    {
        public Vector2[] corners;
        public regionType type;
        public string special, name, toLevel;
        public region(Vector2[] Corners, regionType Type, string Special, string Name, string ToLevel)
        {
            corners = Corners;
            type = Type;
            special = Special;
            name = Name;
            toLevel = ToLevel;
        }
    }
    struct level
    {
        public string name;
        public prop[] props;
        public region[] regions;
        public character[] char_living;
        public debris[] debris_current;
        public item[] items;
        public int row, column;
        public bool indoor;

        public level(string Name)
        {
            name = Name;
            props = null;
            regions = null;
            row = column = 0;
            indoor = false;
            debris_current = new debris[0];
            char_living = new character[0];
            items = new item[0];
        }
    }
    public enum regionType
    {
        floor = 0, building = 1, portal = 3
    };
    #endregion
    #region Weapons
    public struct weapon
    {
        public string name;
        public Texture2D bolt_back, bolt_forward, clip;
        public Texture2D[] muzzleFlash;
        public Vector2 item_pos;
        public Vector2 muzzPos, gripPos, clipPos, actionPos;
        public ProjectileType Pt;
        public StanceType Stance;
        public int clipSize, clipCurrent;
        public float inaccuracy, wear, mass, angle;
        public int ROFdelay, timeWaited;
        public bool auto, jammed, reloading, shooting;

        public weapon(string Name, Texture2D Bolt_Back, Texture2D Bolt_Forward, Texture2D Clip, Texture2D[] MuzzleFlash, Vector2 grip, Vector2 muzzle, Vector2 clip_pos, Vector2 action,
                             ProjectileType PType, StanceType stance, int ClipSize, float Inaccuracy, float Wear, int ROF, bool Auto)
        {
            bolt_back = Bolt_Back;
            bolt_forward = Bolt_Forward;
            clip = Clip;
            item_pos = Vector2.Zero;
            muzzPos = muzzle;
            muzzleFlash = MuzzleFlash;
            clipPos = clip_pos;
            gripPos = grip;
            actionPos = action;
            Pt = PType;
            clipSize = ClipSize;
            clipCurrent = 0;
            inaccuracy = Inaccuracy;
            wear = Wear;
            ROFdelay = ROF;
            timeWaited = 0;
            auto = Auto;
            reloading = shooting = jammed = false;
            name = Name;
            mass = 4;
            angle = 0;
            Stance = StanceType.unarmed;
        }
    }
    public struct Projectile
    {
        public Base B_base;
        public Texture2D texture;
        public ProjectileType type;
        public float lived, maxLife;
        public int damage;
        public int owner;
        public Projectile(Texture2D Texture, Base b_base, ProjectileType Type, float Life)
        {
            texture = Texture;
            B_base = b_base;
            type = Type;
            lived = 0;
            damage = 0;
            maxLife = Life;
            owner = -1;
        }
    }
    public enum StanceType
    {
        Gun_oneHanded,
        Gun_twoHanded,
        Melee_oneHanded,
        Melee_twoHanded,
        unarmed
    };
    public enum ProjectileType
    {
        AR223 = 0,      //.223 ammo for assult rifles
        AR762,          //7.62 ammo for assult rifles
        CAR22,          //.22 ammo for carbines and pistols
        SR50,           //.50 ammo for sniper rifles
        SR308,          //.308 ammo for snipers and carbines
        SG_pellet,      //generic pellet for shot guns
        SMG9,           //9mm ammo for smgs and pistols
        SMG45,          //.45cal ammo for smgs and pistols
        P357,           //.357 ammo for pistols
        P38,             //.38spec ammo for pistols
        debug

    };
    #endregion
    #region ItemsAndInventory
    public struct item
    {
        public Base B_base;
        public Texture2D texture;
        public Texture2D icon;
        public string name;
        public string description;
        public string gameTag;
        public string special;
        public float quality;
        public float weight;
        public itemType type;
        public bool usable; //can you pick it up?
        public item(Base B_Base, Texture2D Tex, Texture2D Ico, string Name, string tag, string Spec, float Quality, itemType Type)
        {
            B_base = B_Base;
            texture = Tex;
            icon = Ico;
            name = Name;
            gameTag = tag;
            quality = Quality;
            special = Spec;
            type = Type;
            description = " ";
            weight = 1;
            usable = true;
        }

    }
    public struct inventory
    {
        public float maxWeight, CurWeight;
        public item[] items;
        public inventory(float MaxWeight, item[] Items)
        {
            maxWeight = MaxWeight;
            CurWeight = 0;
            try
            {
                items = new item[Items.Length];

                for (int i = 0; i != Items.Length; i++)
                {
                    items[i] = Items[i];
                    CurWeight += Items[i].B_base.f_mass;
                }
            }
            catch
            {
                items = new item[0];
                CurWeight = 0;
            }
        }
    }
    public enum itemType
    {
        food,
        weapon,
        medicine,
        ammo,
        tool,
        container,
        armor,
        junk
    };
    #endregion
    #region Effects
    public struct debris
    {
        public Vector2 position;
        public float angle;
        public Texture2D texture;
        public Color color;
        public debrisType type;
        public debris(Vector2 pos, Texture2D tex, Color col, debrisType Type)
        {
            position = pos;
            texture = tex;
            color = col;
            type = Type;
            angle = 0;
        }
    }
    public enum debrisType
    {
        blood,
        gib,
        shell,
        wood,
        metal
    };
    #endregion
    #region AI
    public struct AInode
    {
        public bool active;
        public int tick_last;
        public aiPackage aip_current;
        public aiPackage aip_secondary;
        public aiModifier aim_current;
        public aiSenses ais_senses;
        public Vector2 destination;
        public string current_package_tag;
        public string secondary_package_tag;
        public AInode(bool Active, aiPackage AIpackage, aiModifier AImodifier, aiSenses AIsenses)
        {
            tick_last = 0;
            active = Active;
            aip_current = AIpackage;
            aip_secondary = aiPackage.idle;
            aim_current = AImodifier;
            ais_senses = AIsenses;
            current_package_tag = secondary_package_tag = "";
            destination = Vector2.Zero;
        }
        public AInode(bool Active, aiPackage AIpackage, aiSenses AIsenses)
        {
            tick_last = 0;
            active = Active;
            aip_current = AIpackage;
            aip_secondary = aiPackage.idle;
            aim_current = aiModifier.relaxed;
            ais_senses = AIsenses;
            current_package_tag = secondary_package_tag = "";
            destination = Vector2.Zero;
        }
        public AInode(bool Active, aiPackage AIpackage)
        {
            tick_last = 0;
            active = Active;
            aip_current = AIpackage;
            aip_secondary = aiPackage.idle;
            aim_current = aiModifier.relaxed;
            ais_senses = new aiSenses(350, 700, 20, .5f, .75f, 1);
            current_package_tag = secondary_package_tag = "";
            destination = Vector2.Zero;
        }
        public AInode(bool Active)
        {
            tick_last = 0;
            active = Active;
            aip_current = aiPackage.idle;
            aip_secondary = aiPackage.idle;
            aim_current = aiModifier.relaxed;
            ais_senses = new aiSenses(350, 700, 20, .5f, .75f, 1);
            current_package_tag = secondary_package_tag = "";
            destination = Vector2.Zero;
        }

    }
    public struct aiSenses
    {
        public float radius_hearing;
        public float radius_sight;
        public float radius_touch;

        public float detect_prob_hearing;
        public float detect_prob_sight;
        public float detect_prob_touch;
        public aiSenses(float rad_hearing, float rad_sight, float rad_touch, float prob_hearing, float prob_sight, float prob_touch)
        {
            radius_hearing = rad_hearing;
            radius_sight = rad_sight;
            radius_touch = rad_touch;

            detect_prob_hearing = prob_hearing;
            detect_prob_sight = prob_sight;
            detect_prob_touch = prob_touch;
        }
    }
    public enum aiPackage
    {
        idle, wander, talk, travel, flee, follow, maintainPersonal, trade, attack, pickupItem,keepDistance,turnIntoZomb,none
    };
    public enum aiModifier
    {
        relaxed, sneak, run, rage
    };
    #endregion
    public struct Base
    {
        public Vector2 v_position;
        public float f_Position_Z;
        public float f_Velocity_Z;
        public float f_angle;
        public Vector2 v_velocity;
        public float f_angular_velocity;
        public float f_mass;
        public float f_elasticity;
        public Vector2[] v_collison_map;
        public bool airBorn;

        public Base(Vector2 position, float mass)
        {
            v_position = position;
            f_Position_Z = 0;
            f_Velocity_Z = 0;
            f_elasticity = .1f;
            f_angle = 0;
            v_velocity = Vector2.Zero;
            f_angular_velocity = 0;
            f_mass = mass;
            v_collison_map = new Vector2[1];
            v_collison_map[0] = Vector2.Zero;
            airBorn = false;
        }
        public Base(Vector2 position, float Z, float angle, float mass)
        {
            v_position = position;
            f_Position_Z = 0;
            f_Velocity_Z = 0;
            f_elasticity = .1f;
            f_angle = 0;
            v_velocity = Vector2.Zero;
            f_angular_velocity = 0;
            f_mass = mass;
            v_collison_map = new Vector2[1];
            v_collison_map[0] = Vector2.Zero;
            airBorn = false;
        }
        public Base(Vector2 position, float Z, float angle, Vector2 velocity, float angularVelocity, float mass)
        {
            v_position = position;
            f_Position_Z = Z;
            f_Velocity_Z = 0;
            f_elasticity = .1f;
            f_angle = angle;
            v_velocity = velocity;
            f_angular_velocity = angularVelocity;
            f_mass = mass;
            v_collison_map = new Vector2[1];
            v_collison_map[0] = Vector2.Zero;
            airBorn = false;
        }
    }


}
